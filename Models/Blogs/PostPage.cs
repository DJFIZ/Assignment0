using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment0.Models.Blogs
{
    public class PostPage
    {
        public class Query : IRequest<Command>
        { }

        public class Command : IRequest<CommandResult>
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Body { get; set; }


            //
            // Ambient metadata - stuff needed to complete the postback, but not sent as part of the form.
            //

            public string LoggedInUserName { get; set; }
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

                await SavePostAsync(message, cancellationToken);

                return result;
            }

            private async Task SavePostAsync(Command message, CancellationToken cancellationToken)
            {
                Blog blog = new()
                {
                    BlogId = default,
                    Title = message.Title,
                    Author = message.LoggedInUserName,
                    Body = message.Body,
                    Date = DateTime.Now,
                    NumComments = 0,
                };

                _context.Blogs.Add(blog);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
