using System;
using System.Threading;
using System.Threading.Tasks;
using EventsFacade;
using OperationContracts;
using OperationContracts.Enums;

namespace CommandHandler.Handlers
{
    public class AddDocumentToAnalysisCommandHandler : IHandler<AddDocumentToAnalysisCommand>
    {
        private readonly AnalysisFacade _analysisFacade;

        public AddDocumentToAnalysisCommandHandler(AnalysisFacade analysisFacade)
        {
            _analysisFacade = analysisFacade;
        }


        public async Task<Result> HandleAsync(AddDocumentToAnalysisCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[VerifyDocumentCommandHandler] received command {command}");

            //INFO: The file should be persisted with key of toAnalysisCommand.TaskId
            await _analysisFacade.SaveDocumentAnalysisStatusChangedEventAsync(
                command.FileId, Guid.Empty, command.IssuedOn, command.UserId, OperationStatus.NotStarted, command.FileToVerify.FileName);

            return Result.Success();
        }
    }
}