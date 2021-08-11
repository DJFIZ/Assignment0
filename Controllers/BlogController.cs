using Assignment0.Models;
using Assignment0.VIewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        
        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult List()
        {
            BlogsListViewModel blogsListViewModel = new BlogsListViewModel();
            blogsListViewModel.Blogs = _blogRepository.AllBlogs;
            return View(blogsListViewModel);
        }
    }
}
