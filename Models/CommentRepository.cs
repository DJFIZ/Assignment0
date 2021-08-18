using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment0.Models
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Comment> AllComments 
        {
            get
            {
                return _appDbContext.Comments;
            }
        }

        public Comment GetCommentById(int commentId)
        {
            return _appDbContext.Comments.FirstOrDefault(c => c.commentId == commentId);
        }
    }
}
