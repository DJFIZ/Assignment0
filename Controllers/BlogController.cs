using Assignment0.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogRepository _blogRepository;
        
        public BlogController(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult List()
        {
            return View(_blogRepository.AllBlogs);
        }
    }
}
