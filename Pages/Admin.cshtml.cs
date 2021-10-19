using Assignment0.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment0.Pages
{
    [Authorize]
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _context;

        public AdminModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Blog> Blogs { get; set; }

        public async Task OnGetAsync()
        {
            Blogs = await _context.Blogs.ToListAsync();
        }
    }
}
