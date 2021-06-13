using System;
using System.Linq;
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
            await UpdateFileStatus(command, OperationStatus.NotInitialized);
            
            var result = PythonRunner.Run(
                _scriptsConfiguration.UploadAnalysis,
                $"{command.UserId} {command.FileId} Repository  {command.FileToVerify.FileName} {command.FileToVerify.Content}"
            );
            Console.WriteLine(result);
            if (result.TrimEnd().Split("\n").Last() == "-1")
            {
                Console.WriteLine($"[AddDocumentToAnalysisCommandHandler] upload failed {command}");
                await UpdateFileStatus(command, OperationStatus.Failed);
                return Result.Fail($"Upload of file {command.FileId} failed");
            }
            Console.WriteLine($"[AddDocumentToAnalysisCommandHandler] upload successful");
            //INFO: The file should be persisted with key of toAnalysisCommand.TaskId
            await UpdateFileStatus(command, OperationStatus.NotStarted);
            return Result.Success();
        }

        private async Task UpdateFileStatus(AddDocumentToAnalysisCommand command, OperationStatus status)
        {
            await _analysisFacade.SaveDocumentAnalysisStatusChangedEventAsync(
                command.FileId, Guid.Empty, command.UserId, status, 0.0,
                command.FileToVerify.FileName);
        }
    }
}