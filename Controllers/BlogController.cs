using Assignment0.Models;
using Assignment0.Models.Blogs;
using Assignment0.VIewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Edit(BlogDetails.Query query)
        {
            var model = await _mediator.Send(query);

            if (model.Blog is null)
            {
                return NotFound();
            }

            return View(model);
        }

        public async Task<IActionResult> Comment(PostComment.Command command)
        {
            var result = await _mediator.Send(command);

            return RedirectToAction("Details", "Blog", new { command.Id });
        }

        public async Task<IActionResult> PublishEdit(PostEdit.Command command)
        {
            var result = await _mediator.Send(command);

            return Redirect("/Admin");
        }

        public async Task<IActionResult> Delete(BlogDelete.Command command)
        {
            var result = await _mediator.Send(command);

            return Redirect("/Admin");
        }
    }
}
