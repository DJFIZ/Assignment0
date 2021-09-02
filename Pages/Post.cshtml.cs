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
                await SavePostAsync(NewPost, true);
                return Redirect("/Index");
            }

            return Page();
        }

        [HttpPost]
        public async Task<IActionResult> OnPostSaveDraftAsync()
        {
            if (ModelState.IsValid)
            {
                await SavePostAsync(NewPost, false);
                return Redirect("/Drafts");
            }

            return Page();
        }

        private async Task SavePostAsync(NewPostViewModel newPost, bool publishPost)
        {

            Blog blog = new Blog
            {
                Id = default,
                Title = newPost.Title,
                Author = "TMP",
                Body = newPost.Body,
                Date = DateTime.Now,
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