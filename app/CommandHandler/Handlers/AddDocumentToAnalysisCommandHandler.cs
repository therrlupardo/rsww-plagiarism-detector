using System;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler.Configuration;
using Common.Utils;
using EventsFacade;
using Microsoft.Extensions.Options;
using OperationContracts;
using OperationContracts.Enums;

namespace CommandHandler.Handlers
{
    public class AddDocumentToAnalysisCommandHandler : IHandler<AddDocumentToAnalysisCommand>
    {
        private readonly AnalysisFacade _analysisFacade;
        private readonly Scripts _scriptsConfiguration;

        public AddDocumentToAnalysisCommandHandler(AnalysisFacade analysisFacade, IOptions<Scripts> scriptsConfiguration)
        {
            _analysisFacade = analysisFacade;
            _scriptsConfiguration = scriptsConfiguration.Value;
        }


        public async Task<Result> HandleAsync(AddDocumentToAnalysisCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AddDocumentToAnalysisCommandHandler] received command {command}");

            var result = PythonRunner.Run(
                _scriptsConfiguration.UploadAnalysis,
                $"{command.UserId} {command.FileId} Repository  {command.FileToVerify.FileName} {command.FileToVerify.Content}"
            );
            Console.WriteLine($"[AddDocumentToAnalysisCommandHandler] upload result {result}");
            //INFO: The file should be persisted with key of toAnalysisCommand.TaskId
            await _analysisFacade.SaveDocumentAnalysisStatusChangedEventAsync(
                command.FileId, Guid.Empty, command.IssuedOn, command.UserId, OperationStatus.NotStarted, null, command.FileToVerify.FileName);

            return Result.Success();
        }
    }
}