using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;

namespace CommandHandler.Handlers
{
    public class VerifyDocumentCommandHandler : IHandler<AddDocumentToAnalysisCommand>
    {
        public async Task<Result> HandleAsync(AddDocumentToAnalysisCommand toAnalysisCommand, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[VerifyDocumentCommandHandler] received command {toAnalysisCommand}");


            return Result.Success();
        }
    }
}