using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;

namespace CommandService.Handlers
{
    public class VerifyTestHandler : IHandler<VerifyDocumentCommand>
    {
        public async Task<Task> HandleAsync(VerifyDocumentCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[VerifyTestHandler] received command {command}");
            return Task.CompletedTask;
        }
    }
}