using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;

namespace CommandHandler.Handlers
{
    public class VerifyDocumentCommandHandler : IHandler<VerifyDocumentCommand>
    {
        public async Task<Task> HandleAsync(VerifyDocumentCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[VerifyDocumentCommandHandler] received command {command}");
            return Task.CompletedTask;
        }
    }
}