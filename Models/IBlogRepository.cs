﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public interface IBlogRepository
    {
        IEnumerable<Blog> AllBlogs { get; }
        Blog GetBlogById(int blogId);
    }
}
