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

        // Common practice is to always initialize a collection property, even if you know it will
        //   be empty. This prevents the issue you're seeing when you get a null reference exception
        //   when trying to foreach over the comments.
        //
        // IMO, it's okay to use List<T> instead of IList<T>.
        //
        // Ex: public List<Comment> Comments { get; private set; } = new();
        //
        // The private setter means that no caller external to this class can set it to null or 
        //   otherwise change the reference.
        //
        // This also means that if you need to add items to this collection, you can't initialize it
        //   to a new List<T>. You'll have to use the .Add() or .AddRange() methods instead.
        public IList<Comment> Comments { get; set; }
    }


}
