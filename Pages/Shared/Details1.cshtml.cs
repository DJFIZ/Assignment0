using System.Threading.Tasks;
using Assignment0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Assignment0.Pages.Shared
{
    public class Details1Model : PageModel
    {
        private readonly AppDbContext _context;

        public Details1Model(AppDbContext context)
        {
            _context = context;
        }

        public Blog Blog { get; set; }


        public async Task OnGetAsync()
        {
            
        }
    }
}
