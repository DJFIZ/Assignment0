using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Assignment0.Models.Blogs
{
    public class CommentDelete
    {
        public class Query : IRequest<Command>
        {   }

        public class Command : IRequest<CommandResult>
        {
            public int Id { get; set; }
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

                await DeleteCommentAsync(message, cancellationToken);

                return result;
            }

            private async Task DeleteCommentAsync(Command message, CancellationToken cancellationToken)
            {
                var comment = await _context.Comments
                .SingleOrDefaultAsync(c => c.CommentId == message.Id, cancellationToken);

                if (comment != null)
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
