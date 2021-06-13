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

            await UpdateAnalysisStatus(command, OperationStatus.Running);

            var result = double.Parse(PythonRunner.Run(
                _scriptsConfiguration.PerformAnalysis,
                command.FileId.ToString()
                ));
            if (result < 0)
            {
                Console.WriteLine($"[{nameof(AnalyzeDocumentCommandHandler)}] Analysis for task {command.TaskId} failed with code {result}");
                await UpdateAnalysisStatus(command, OperationStatus.Failed);
                return Result.Fail($"Analysis for task {command.TaskId} failed with code {result}");
            }
            Console.WriteLine($"[{nameof(AnalyzeDocumentCommandHandler)}] analysis result {result}");
            
            //TODO: Wrap it in a service and send the notification to the frontend
            await UpdateAnalysisStatus(command, OperationStatus.Complete, result);

            return Result.Success();
        }

        private async Task UpdateAnalysisStatus(AnalyzeDocumentCommand command, OperationStatus status,  double result = 0.0)
        {
            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(command.FileId, command.TaskId,
                command.IssuedTime,
                command.UserId, status, result);
        }
    }
}