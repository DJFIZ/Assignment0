using Assignment0.Models;
using Assignment0.VIewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        //  IActionResult is a base interface that I tend to use as the return type of Actions in case
        //  I have an if statement that returns different results (i.e., ViewResult vs. ContentResult).
        public async Task<IActionResult> List()
        {
            BlogsListViewModel blogsListViewModel = new(); // This is new C# 9 syntax; not required, but I like it.

            //  Use the async version of EF's querying methods.
            blogsListViewModel.Blogs = await _appDbContext.Blogs.ToArrayAsync();

            return View(blogsListViewModel);
        }
    }
}
