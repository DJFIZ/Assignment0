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
    public class CommentEdit
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
                var comment = await _context.Comments
                .SingleOrDefaultAsync(c => c.CommentId == message.Id, cancellationToken);

                 if (comment == null)
                {
                    //return NotFoundException();
                }

                comment.Author = HtmlEncode(message.Name, true);
                comment.Body = HtmlEncode(message.Comment, true);

                // When you're updating an existing entity, you don't need to add it back to the context.
                // Also, EF Core recommends against using "AddAsync". This is one of the rare exceptions to
                //   preferring the Async version of methods when writing an ASP.NET Core app.
                // From the comments on the "AddAsync" method:
                //   This method is async only to allow special value generators, such as the one
                //     used by 'Microsoft.EntityFrameworkCore.Metadata.SqlServerValueGenerationStrategy.SequenceHiLo',
                //     to access the database asynchronously. For all other cases the non async method
                //     should be used.
                //await _context.Comments.AddAsync(comment, cancellationToken);
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
