using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Assignment0.Models;

namespace Assignment0.Pages
{
    //[Authorize]
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

 
        public IActionResult OnPostPublish()
        {
            if (ModelState.IsValid)
            {
                SavePost(NewPost, true);
                return Redirect("/Index");
            }

            return Page();
        }

        [HttpPost]
        public IActionResult OnPostSaveDraft()
        {
            if (ModelState.IsValid)
            {
                SavePost(NewPost, false);
                return Redirect("/Drafts");
            }

            return Page();
        }

        private void SavePost(NewPostViewModel newPost, bool publishPost)
        {

            Blog blog = new Blog
            {
                Id = 12,
                Title = newPost.Title,
                Author = "TMP",
                Body = newPost.Body,
                Date = DateTimeOffset.Now.ToString(),
            };

            _context.Blogs.Add(blog);

            // Missing a call to SaveChangesAsync(), but otherwise, looks good. Try to make it async.
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