using Assignment0.Models.Blogs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Assignment0.Pages
{
    [Authorize]
    public class PostModel : PageModel
    {
        private readonly IMediator _mediator;

        public PostModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        public PostPage.Command NewPost { get; set; }

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostPublish()
        {
            NewPost.LoggedInUserName = User.Identity.Name;

            if (ModelState.IsValid)
            {
                // We don't do anything with this result, but you can use it to pass back
                //   data for the postback response.
                var result = await _mediator.Send(NewPost);

                return Redirect("/Index");
            }

            return Page();
        }

        ////[HttpPost]
        //public async Task<IActionResult> OnPostSaveDraftAsync()
        //{
        //    if (ModelState.IsValid)
        //    {
        //        await SavePostAsync(NewPost);
        //        return Redirect("/Drafts");
        //    }

        //    return Page();
        //}
    }
}