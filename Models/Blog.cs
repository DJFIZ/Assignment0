using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment0.Models
{
    public class Blog
    {
        public int BlogId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Body { get; set; }

        public int NumComments { get; set; }

        public List<Comment> Comments { get; set; } = new();
    }


}
