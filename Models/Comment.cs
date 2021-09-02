using System;
using System.ComponentModel.DataAnnotations;

namespace Assignment0.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int BlogId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Body { get; set; }
    }
}
