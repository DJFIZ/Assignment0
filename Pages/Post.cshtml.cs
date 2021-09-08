using Assignment0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Assignment0.Pages
{
    [Authorize]
    public class PostModel : PageModel
    {
        private readonly AppDbContext _context;

        public PostModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public NewPostViewModel NewPost { get; set; }

        public void OnGet()
        {

        }


        public async Task<IActionResult> OnPostPublish()
        {
            if (ModelState.IsValid)
            {
                await SavePostAsync(NewPost);
                return Redirect("/Index");
            }

            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostSaveDraftAsync()
        {
            if (ModelState.IsValid)
            {
                await SavePostAsync(NewPost);
                return Redirect("/Drafts");
            }

            return Page();
        }

        private async Task SavePostAsync(NewPostViewModel newPost)
        {

            Blog blog = new()
            {
                BlogId = default,
                Title = newPost.Title,
                // With ASP.NET Core Identity, the email address is stored in the User.Identity.Name
                //   property when the user is logged in. Not intuitive; I think this is a holdover from
                //   the olden days of .NET Framework when they first introduced ClaimsPrincipal, which is
                //   the C# type of User.
                //Author = this.User.ToString(),
                Author = User.Identity.Name,
                Body = newPost.Body,
                Date = DateTime.Now,
                NumComments = 0,
            };

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
        }

        public class NewPostViewModel
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Body { get; set; }
        }
    }
}