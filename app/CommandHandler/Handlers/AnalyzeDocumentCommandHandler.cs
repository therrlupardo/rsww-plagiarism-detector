using System;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler.Configuration;
using Common.Utils;
using EventsFacade;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using OperationContracts;
using OperationContracts.Enums;

namespace CommandHandler.Handlers
{
    public class AnalyzeDocumentCommandHandler : IHandler<AnalyzeDocumentCommand>
    {
        private readonly AnalysisFacade _analyzeEventsFacade;
        private readonly Scripts _scriptsConfiguration;

        public AnalyzeDocumentCommandHandler(AnalysisFacade analyzeEventsFacade, IOptions<Scripts> scriptsConfiguration)
        {
            _analyzeEventsFacade = analyzeEventsFacade;
            _scriptsConfiguration = scriptsConfiguration.Value;
        }

        public async Task<Result> HandleAsync(AnalyzeDocumentCommand command, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(AnalyzeDocumentCommandHandler)} received command {command} ");

            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(
                command.FileId, command.TaskId, command.IssuedTime, command.UserId, OperationStatus.Running, null);

            // TODO: Perform the analysis here
            var result = PythonRunner.Run(
                _scriptsConfiguration.PerformAnalysis,
                ""
                );
            Console.WriteLine($"[{nameof(AnalyzeDocumentCommandHandler)}] analysis result {result}");
            
            //TODO: Wrap it in a service and send the notification to the frontend
            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(command.FileId, command.TaskId, command.IssuedTime,
                command.UserId, OperationStatus.Complete, result);

            return Result.Success();
        }
    }
}