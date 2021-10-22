using Assignment0.Models;
using Assignment0.Models.Blogs;
using AutoMapper;

namespace Assignment0.MappingProfiles
{
    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            // Map from Models.Blog to BlogDetails.Command.BlogModel
            CreateMap<Blog, BlogDetails.Command.BlogModel>();

            // Map from Models.Comment to BlogDetails.Command.CommentModel
            CreateMap<Comment, BlogDetails.Command.CommentModel>();
        }
    }
}
