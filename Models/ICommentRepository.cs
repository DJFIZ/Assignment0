using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> AllComments { get; }
        Comment GetCommentById(int commentId);
    }
}
