using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;
using System.Threading.Tasks;


namespace Assignment0.Models.Blogs
{
    public class CommentPost
    {
        public class Query : IRequest<Command>
        { }

        public class Command : IRequest<CommandResult>
        {
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [Required]
            public string Comment { get; set; }
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

                await CommentAsync(message, cancellationToken);

                return result;
            }

            private async Task CommentAsync(Command message, CancellationToken cancellationToken)
            {
                var blog = await _context.Blogs
                    .Include(b => b.Comments)
                    .SingleOrDefaultAsync(b => b.BlogId == message.Id, cancellationToken);


                Console.WriteLine(message.Comment);

                var c = new Comment
                {
                    BlogId = message.Id,
                    Author = HtmlEncode(message.Name, true),
                    Body = HtmlEncode(message.Comment, true),
                    Date = DateTime.Now
                };

                blog.NumComments++;
                await _context.Comments.AddAsync(c, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            public static string HtmlEncode(string text, bool preserveNewlines)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return text;
                }

                // HTML encode everything first.
                var encoded = WebUtility.HtmlEncode(text);

                if (preserveNewlines)
                {
                    // Convert newlines to <br /> tags.
                    encoded = encoded
                        .Replace("\r\n", "\n")    // Make "\r\n" into "\n"
                        .Replace("\n", "<br />"); // Make "\n" into "<br />"
                }

                return encoded;
            }
        }
    }
}
