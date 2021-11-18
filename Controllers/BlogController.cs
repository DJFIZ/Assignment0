using Assignment0.Models;
using Assignment0.Models.Blogs;
using Assignment0.VIewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Security.Application;
using System.Threading.Tasks;
using System.Web;

namespace Assignment0.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMediator _mediator;
        //private readonly AntiXssEncoder _antiXssEncoder;

        public BlogController(AppDbContext appDbContext, IMediator mediator)
        {
            _appDbContext = appDbContext;
            _mediator = mediator;
        }

        public async Task<IActionResult> List()
        {
            BlogsListViewModel blogsListViewModel = new(); // This is new C# 9 syntax; not required, but I like it.

            blogsListViewModel.Blogs = await _appDbContext.Blogs.ToArrayAsync();
            blogsListViewModel.Comments = await _appDbContext.Comments.ToArrayAsync();

            return View(blogsListViewModel);
        }

        public async Task<IActionResult> Details(BlogDetails.Query query)
        {
            // This executes the code in the BlogDetails.QueryHandler class and returns the result.
            var model = await _mediator.Send(query);

            if (model.Blog is null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _appDbContext.Blogs
                .Include(b => b.Comments)
                .AsNoTracking()
                .SingleOrDefaultAsync(b => b.BlogId == id);

            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        public async Task<IActionResult> Comment(int id, string name, string comment)
        {
            var blog = await _appDbContext.Blogs
                .Include(b => b.Comments)
                .SingleOrDefaultAsync(b => b.BlogId == id);

            var c = new Comment { BlogId = id, Author = HttpUtility.HtmlEncode(name), Body = HttpUtility.HtmlEncode(comment), Date = System.DateTime.Now };

            blog.NumComments++;
            await _appDbContext.Comments.AddAsync(c);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Details", "Blog", new { id });
        }

        public async Task<IActionResult> PublishEdit(int id, string title, string body)
        {
            var blog = await _appDbContext.Blogs
                .Include(b => b.Comments)
                .SingleOrDefaultAsync(b => b.BlogId == id);

            if (blog == null)
            {
                return NotFound();
            }

            blog.Title = title;
            blog.Body = body;
            await _appDbContext.SaveChangesAsync();

            return Redirect("/Admin");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _appDbContext.Blogs
                .Include(b => b.Comments)
                .SingleOrDefaultAsync(b => b.BlogId == id);

            if (blog != null)
            {
                _appDbContext.Blogs.Remove(blog);
                await _appDbContext.SaveChangesAsync();
            }

            return Redirect("/Admin");
        }
    }
}
