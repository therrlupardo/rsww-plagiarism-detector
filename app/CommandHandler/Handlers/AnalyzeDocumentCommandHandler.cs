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

            try
            {
                var result = getAnalysisResult(command);
                Console.WriteLine($"[{nameof(AnalyzeDocumentCommandHandler)}] analysis result {result}");

                //TODO: Wrap it in a service and send the notification to the frontend
                await UpdateAnalysisStatus(command, OperationStatus.Complete, result);

                return Result.Success();
            }
            catch (NotSupportedException e)
            {
                Console.WriteLine(
                    $"[{nameof(AnalyzeDocumentCommandHandler)}] Analysis for task {command.TaskId} failed.");
                await UpdateAnalysisStatus(command, OperationStatus.Failed);
                return Result.Fail($"Analysis for task {command.TaskId} failed.");
            }
        }

        private async Task UpdateAnalysisStatus(AnalyzeDocumentCommand command, OperationStatus status,
            double result = 0.0)
        {
            await _analyzeEventsFacade.SaveDocumentAnalysisStatusChangedEventAsync(command.FileId, command.TaskId,
                command.UserId, status, result);
        }

        private double getAnalysisResult(AnalyzeDocumentCommand command)
        {
            var scriptResult = PythonRunner.Run(
                _scriptsConfiguration.PerformAnalysis,
                command.FileId.ToString()
            );
            Console.WriteLine(scriptResult);
            var last = scriptResult.TrimEnd().Split("\n").Last();
            if (double.TryParse(last, out var result))
            {
                return result;
            }
            throw new NotSupportedException(scriptResult);
        }
    }
}