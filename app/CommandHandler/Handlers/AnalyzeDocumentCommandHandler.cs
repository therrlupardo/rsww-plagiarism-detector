using System;
using System.Threading;
using System.Threading.Tasks;
using Commands;
using EventsFacade;
using Queries.Enums;

namespace CommandHandler.Handlers
{
    public class AnalyzeDocumentCommandHandler : IHandler<AnalyzeDocumentCommand>
    {
        private readonly AnalysisFacade _analyzeEventsFacade;

        public AnalyzeDocumentCommandHandler(AnalysisFacade analyzeEventsFacade)
        {
            _analyzeEventsFacade = analyzeEventsFacade;
        }

        public async Task<Result> HandleAsync(AnalyzeDocumentCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(AnalyzeDocumentCommandHandler)} received command {command} ");

            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(command.FileId, command.IssuedTime,
                command.UserId, OperationStatus.Running);

            return Result.Success();
        }
    }
}