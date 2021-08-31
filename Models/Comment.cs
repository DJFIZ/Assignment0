using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public class Comment
    {
        // Generally speaking, prefer your C# property names to be pascal-cased

        // CommentId
        public int commentId { get; set; }

        // BlogId
        public int blogId { get; set; }

        public string Author { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
    }
}
