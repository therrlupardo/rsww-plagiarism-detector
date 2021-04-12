using System;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Services;
using Queries.Enums;

namespace EventsFacade
{
    public class AnalysisFacade
    {
        private readonly IDocumentAnalysisService _analysisService;

        public AnalysisFacade(IDocumentAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public async Task SaveDocumentAnalysisStatusChangedEventAsync(Guid fileId, DateTime occurenceDate, Guid userId, OperationStatus status)
        {
            var @event = new DocumentAnalysisStatusChangedEvent
            {
                Status = status,
                FileId = fileId,
                OccurenceDate = occurenceDate
            };

            await _analysisService.SaveAnalysisStatusChanged(@event, userId);
        }
    }
}