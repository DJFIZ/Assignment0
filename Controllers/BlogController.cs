using Assignment0.Models;
using Assignment0.VIewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
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
        public async Task<IActionResult> Details(int id)
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
    }
}
