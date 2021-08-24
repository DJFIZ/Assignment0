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

        /*
        I don't use repositories. The EF context is already a repository, so I just inject that
        directly. Plus, if you need to access lower-level functionality of the context (usually
        if you need to do something tricky, or you need to execute raw SQL), you don't have to 
        figure out how to expose it via both your repository interface and its concrete 
        implementation; it's just already right there on the context.
        
        When building websites, especially when the pages talk to databases and APIs that live
        elsewhere on the network, always strongly prefer to use C#'s async/await feature.
        
        Without async/await, any call you make to SQL Server will block a thread while waiting
        for a response. That thread can't respond to other requests while it's waiting for the
        response. It's wasteful.

        With async/await, ASP.NET will fire off a request to SQL Server, and then return the
        thread back to the thread pool so that it can do something else while you're waiting for
        a response from SQL Server. When the SQL Server response is ready, ASP.NET will get a
        thread from the thread pool, resume where it left off, and allow you to send a response.

        The way this all happens is pretty magical, but I highly recommend you dig into it more.

        So here is how I would rewrite this class:

        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // IActionResult is a base interface that I tend to use as the return type of Actions in case
        //   I have an if statement that returns different results (i.e., ViewResult vs. ContentResult).
        public async Task<IActionResult> List()
        {
            BlogsListViewModel blogsListViewModel = new(); // This is new C# 9 syntax; not required, but I like it.

            // Use the async version of EF's querying methods.
            blogsListViewModel.Blogs = await _appDbContext.Blogs.ToArrayAsync();

            return View(blogsListViewModel);
        }
        */
    }
}
