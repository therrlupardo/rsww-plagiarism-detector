using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;

namespace CommandHandler.Handlers
{
    public class AddDocumentToSourceStoreCommandHandler : IHandler<AddDocumentToSourceStoreCommand>
    {
        public async Task<Task> HandleAsync(AddDocumentToSourceStoreCommand command,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] received command {command}");
            return Task.CompletedTask;
        }
    }
}