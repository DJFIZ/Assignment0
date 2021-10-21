using Assignment0.Models;
using Assignment0.Models.Blogs;
using Assignment0.VIewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Assignment0.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IMediator _mediator;

        public BlogController(AppDbContext appDbContext, IMediator mediator)
        {
            _appDbContext = appDbContext;
            _mediator = mediator;
        }

        //  IActionResult is a base interface that I tend to use as the return type of Actions in case
        //  I have an if statement that returns different results (i.e., ViewResult vs. ContentResult).
        public async Task<IActionResult> List()
        {
            BlogsListViewModel blogsListViewModel = new(); // This is new C# 9 syntax; not required, but I like it.

            blogsListViewModel.Blogs = await _appDbContext.Blogs.ToArrayAsync();
            blogsListViewModel.Comments = await _appDbContext.Comments.ToArrayAsync();

            return View(blogsListViewModel);
        }

        /*
        When accessing any resource that requires I/O, prefer the async version of method calls.

        To access child comments for a blog post, .Include() the navigation property.

        Since BlogId is the primary key on the Blogs database table, use SingleOrDefaultAsync(); there should
        be at most one Blog record with a given id.

        Also, since this action only reads entities from the database, call AsNoTracking() to avoid the
        overhead of tracking the entities in EF Core. You only need entity tracking if you're going to 
        load an entity, change it, and then save it back to the database in a single web request.
        */
        /*
        2021-10-21: I updated this method to use MediatR and view models. Since the data model is so simple,
        the benefits aren't immediately apparent, but this is the method we use to ensure the ultimate flexibility
        in shaping the data for display.

        If this action posted data back to the server, you would see the separation of validation. We'd put the
        validation on the view models instead of relying on the validation on the EF entities. UI validation can
        subtly differ from database validation - for example, a field might be non-nullable in the database but
        nullable in the UI.
        */
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

            var c = new Comment { BlogId = id, Author = name, Body = comment, Date = System.DateTime.Now };

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
