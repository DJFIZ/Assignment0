using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public class MockBlogRepository: BlogRepository
    {
        public IEnumerable<Blog> AllBlogs =>
            new List<Blog>
            {
                new Blog{Id = 1, Title = "LMAO", Author = "me", Body = "sleep", Date = "Today"},
                new Blog{Id = 2, Title = "FUCK", Author = "you", Body = "awake", Date = "Tomorrow"}
            };

        public Blog GetBlogById(int blogId)
        {
            return AllBlogs.FirstOrDefault(b => b.Id == blogId);
        }
    }
}
