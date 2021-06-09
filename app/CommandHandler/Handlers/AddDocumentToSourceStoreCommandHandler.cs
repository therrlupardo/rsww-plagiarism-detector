using System;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler.Configuration;
using Common.Utils;
using EventsFacade;
using Microsoft.Extensions.Options;
using OperationContracts;

namespace CommandHandler.Handlers
{
    public class AddDocumentToSourceStoreCommandHandler : IHandler<AddDocumentToSourceStoreCommand>
    {
        private readonly SourceDocumentFacade _sourceDocumentFacade;
        private readonly Scripts _scriptsConfiguration;

        public AddDocumentToSourceStoreCommandHandler(SourceDocumentFacade sourceDocumentFacade, IOptions<Scripts> scriptsConfiguration)
        {
            _sourceDocumentFacade = sourceDocumentFacade;
            _scriptsConfiguration = scriptsConfiguration.Value;
        }

        public async Task<Result> HandleAsync(AddDocumentToSourceStoreCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] received command {command}");

            var result = PythonRunner.Run(
                _scriptsConfiguration.UploadSource,
                ""
                );
            Console.WriteLine($"[AddDocumentToSourceStoreCommandHandler] upload result {result}");
            
            await _sourceDocumentFacade.SaveDocumentAddedToSource(command.File.FileName, command.FileId, command.UserId);

            return Guid.NewGuid().ToSuccessfulResult();
        }
    }
}