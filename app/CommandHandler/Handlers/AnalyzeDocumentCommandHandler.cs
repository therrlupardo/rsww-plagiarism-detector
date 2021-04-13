using System;
using System.Threading;
using System.Threading.Tasks;
using EventsFacade;
using OperationContracts;
using OperationContracts.Enums;

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

            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(
                command.FileId, command.TaskId, command.IssuedTime, command.UserId, OperationStatus.Running);

            // TODO: Perform the analysis here


            //TODO: Wrap it in a service and send the notification to the frontend
            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(command.FileId, command.TaskId, command.IssuedTime,
                command.UserId, OperationStatus.Complete);

            return Result.Success();
        }
    }
}