using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventsFacade.Events;
using EventsFacade.Services;
using OperationContracts.Enums;

namespace EventsFacade
{
    public class AnalysisFacade
    {
        private readonly IDocumentAnalysisService _analysisService;

        public AnalysisFacade(IDocumentAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        public async Task SaveDocumentAnalysisStatusChangedEventAsync(Guid documentId, Guid taskId, DateTime occurenceDate, Guid userId, OperationStatus status,  double result = 0.0, string documentName = null)
        {
            var @event = new DocumentAnalysisStatusChangedEvent
            {
                DocumentId = documentId,
                DocumentName = documentName,
                TaskId = taskId,
                Status = status,
                OccurenceDate = occurenceDate,
                Result = result
            };

            await _analysisService.SaveAnalysisStatusChanged(@event, userId);
        }

        public async Task<List<DocumentAnalysisStatusChangedEvent>> GetAllUserDocumentAnalysesAsync(Guid userId) => await _analysisService.GetAnalysesForUser(userId);
    }
}