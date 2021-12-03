using Assignment0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assignment0.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public IndexModel(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IList<Blog> Blogs { get; set; }


        public async Task OnGetAsync()
        {
            Blogs = await _context.Blogs.ToListAsync();
        }

    }
}
