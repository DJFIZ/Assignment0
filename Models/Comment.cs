using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public class Comment
    {
        public int commentId { get; set; }
        public int blogId { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }
    }
}
