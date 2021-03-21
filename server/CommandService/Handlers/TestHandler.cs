using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;

namespace CommandService.Handlers
{
    public class TestHandler : IHandler<AddDocumentToSourceStoreCommand>
    {
        public async Task<Task> HandleAsync(AddDocumentToSourceStoreCommand command,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"[TestHandler] received command {command}");
            return Task.CompletedTask;
        }
    }
}