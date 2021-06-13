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
    public class AddDocumentToSourceStoreCommandHandler : IHandler<AddDocumentToSourceStoreCommand>
    {
        private readonly SourceDocumentFacade _sourceDocumentFacade;
        private readonly Scripts _scriptsConfiguration;

        public async Task<Result> HandleAsync(AddDocumentToSourceStoreCommand command,
            CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] received command {command}");

            await UpdateFileStatus(command, OperationStatus.NotInitialized);
            var result = PythonRunner.Run(
                _scriptsConfiguration.UploadSource,
                $"{command.UserId} {command.FileId} Repository  {command.File.FileName} {command.File.Content}"
            );
            if (result is null or "")
            {
                Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] upload result {result}");
                await UpdateFileStatus(command, OperationStatus.Complete);
                return Guid.NewGuid().ToSuccessfulResult();
            }

            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] Upload failed for file {command.FileId}");
            await UpdateFileStatus(command, OperationStatus.Failed);
            return Result.Fail($"Upload failed for file {command.FileId}");
        }

        public AddDocumentToSourceStoreCommandHandler(SourceDocumentFacade sourceDocumentFacade,
            IOptions<Scripts> scriptsConfiguration)
        {
            _sourceDocumentFacade = sourceDocumentFacade;
            _scriptsConfiguration = scriptsConfiguration.Value;
        }

        private async Task UpdateFileStatus(AddDocumentToSourceStoreCommand command, OperationStatus status)
        {
            await _sourceDocumentFacade.SaveDocumentAddedToSource(command.File.FileName, command.FileId, command.UserId,
                status);
        }
    }
}