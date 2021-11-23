using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment0.Models.Blogs
{
    public class BlogDetails
    {
        // This class holds data from the request.
        public class Query : IRequest<Command>
        {
            // This property is the equivalent of "id" in the original Details controller action:
            //   public async Task<IActionResult> Details(int id). ASP.NET will grab the blog entry
            //   id from the route and stick it in this property.
            public int Id { get; set; }

        }

        // This class holds data for the response that we're sending back to the browser. In this case,
        //   that's the blog details.
        public class Command : IRequest<CommandResult>
        {
            public BlogModel Blog { get; set; }


            //
            // Classes
            //

            public class BlogModel
            {
                public int BlogId { get; set; }
                public string Title { get; set; }
                public DateTime Date { get; set; }
                public string Author { get; set; }
                public string Body { get; set; }
                public int NumComments { get; set; }

                public List<CommentModel> Comments { get; private set; } = new();
            }

            public class CommentModel
            {
                public int CommentId { get; set; }
                public string Author { get; set; }
                public DateTime Date { get; set; }
                public string Body { get; set; }
            }
        }

        public class CommandResult
        {
            public List<(string, string)> Errors { get; private set; } = new List<(string, string)>();
        }


        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly AppDbContext _appDbContext;
            private readonly IMapper _mapper;

            public QueryHandler(AppDbContext appDbContext, IMapper mapper)
            {
                _appDbContext = appDbContext;
                _mapper = mapper;
            }

            public async Task<Command> Handle(Query message, CancellationToken cancellationToken)
            {
                var command = new Command();

                var blog = await _appDbContext.Blogs
                    .Include(b => b.Comments)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(b => b.BlogId == message.Id, cancellationToken);

                command.Blog = _mapper.Map<Command.BlogModel>(blog);

                return command;
            }
        }

        // If the Details page posted data to be saved, this would handle the POST request, but we don't,
        //   so we don't need this.
        //public class CommandHandler : IRequestHandler<Command, CommandResult>
        //{
        //    public CommandHandler()
        //    {

        //    }

        //    public async Task<CommandResult> Handle(Command message, CancellationToken cancellationToken)
        //    {
        //        var result = new CommandResult();

        //        await Task.CompletedTask;

        //        return result;
        //    }
        //}
    }
}
