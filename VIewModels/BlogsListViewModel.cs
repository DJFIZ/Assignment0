using Assignment0.Models;
using System.Collections.Generic;

namespace Assignment0.VIewModels
{
    public class BlogsListViewModel
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
