using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    // Repositories are cumbersome. I would use the AppDbContext directly.

    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _appDbContext;

        public BlogRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Blog> AllBlogs 
        {
            get
            {
                return _appDbContext.Blogs;
            }
        }

        public Blog GetBlogById(int blogId)
        {
            return _appDbContext.Blogs.FirstOrDefault(b => b.Id == blogId);
        }
    }
}
