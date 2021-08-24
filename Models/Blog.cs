using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Date { get; set; }
        public string Body { get; set; }

        /*
        
        You can annotate your EF models to be explicit about, e.g., the maximum size of a string column, or
        whether or not a column is allowed to hold NULL values.

        public int Id { get; set; }

        // Here, we're saying that Title cannot be NULL, and it has a max length of 100 characters.
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        // Similar for author.
        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        // Make this an actual DateTime, not a string
        // e.g., public DateTime Date { get; set; }
        public string Date { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Body { get; set; }
        */
    }
}
