using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;
using EventsFacade;

namespace CommandHandler.Handlers
{
    public class AddDocumentToAnalysisCommandHandler : IHandler<AddDocumentToAnalysisCommand>
    {
        private readonly DocumentsToAnalysisFacade _documentsToAnalyzeFacade;

        public AddDocumentToAnalysisCommandHandler(DocumentsToAnalysisFacade documentsToAnalyzeFacade)
        {
            _documentsToAnalyzeFacade = documentsToAnalyzeFacade;
        }

        public async Task<Result> HandleAsync(AddDocumentToAnalysisCommand toAnalysisCommand, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[VerifyDocumentCommandHandler] received command {toAnalysisCommand}");

            //INFO: The file should be persisted with key of toAnalysisCommand.TaskId
            await _documentsToAnalyzeFacade.SaveDocumentToAnalysisAddedAsync(toAnalysisCommand);

            return Result.Success();
        }
    }
}