using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment0.Models.Blogs
{
    public class BlogEdit
    {
        public class Query : IRequest<Command>
        { }

        public class Command : IRequest<CommandResult>
        {
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public string Body { get; set; }
        }

        public class CommandResult
        {
            public List<(string, string)> Errors { get; private set; } = new List<(string, string)>();
        }


        public class QueryHandler : IRequestHandler<Query, Command>
        {
            public QueryHandler()
            {

            }

            public async Task<Command> Handle(Query message, CancellationToken cancellationToken)
            {
                var command = new Command();

                await Task.CompletedTask;

                return command;
            }
        }

        public class CommandHandler : IRequestHandler<Command, CommandResult>
        {
            private readonly AppDbContext _context;

            public CommandHandler(AppDbContext context)
            {
                _context = context;
            }

            public async Task<CommandResult> Handle(Command message, CancellationToken cancellationToken)
            {
                var result = new CommandResult();

                await EditAsync(message, cancellationToken);

                return result;
            }

            private async Task EditAsync(Command message, CancellationToken cancellationToken)
            {
                var blog = await _context.Blogs
                    .Include(b => b.Comments)
                    .SingleOrDefaultAsync(b => b.BlogId == message.Id, cancellationToken);

                if (blog == null)
                {
                    //return NotFoundException();
                }

                blog.Title = message.Title;
                blog.Body = message.Body;

                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
