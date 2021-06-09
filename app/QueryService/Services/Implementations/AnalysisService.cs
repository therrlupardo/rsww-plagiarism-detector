using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade;
using EventsFacade.Events;
using OperationContracts.Enums;

namespace QueryService.Services.Implementations
{
    internal class AnalysisService : IAnalysisService
    {
        private readonly AnalysisFacade _facade;

        public AnalysisService(AnalysisFacade facade)
        {
            _facade = facade;
        }

        public async Task<List<AnalysisStatusDto>> GetUserAnalysesAsync(Guid userId)
        {
            var allAnalysesEvents = await _facade.GetAllUserDocumentAnalysesAsync(userId);


            DocumentAnalysisStatusChangedEvent GetInitialEvent(Guid docId) =>
                allAnalysesEvents.FirstOrDefault(e => e.Status == OperationStatus.NotStarted && e.DocumentId == docId);

            var analysesAndRelatedEvents = allAnalysesEvents
                .Where(a => a.Status != OperationStatus.NotStarted)
                .GroupBy(a => a.TaskId);


            return analysesAndRelatedEvents.Select(events =>
                    events.OrderByDescending(e => e.Status).First())
                .Select(latestEvent => new AnalysisStatusDto(latestEvent))
                .Select(a => a with { DocumentName = GetInitialEvent(a.DocumentId)?.DocumentName ?? "No info"})
                .ToList();
        }


        public async Task<AnalysisStatusDto> GetByIdAsync(Guid userId, Guid taskId)
        {
            var latestStatus =
                await GetLatestStatusChangeByAsync((e => e.TaskId == taskId), userId);

            return new(latestStatus);
        }

        public async Task<AnalysisStatusDto> GetByDocumentIdAsync(Guid userId, Guid documentId)
        {
            var latestStatus =
                await GetLatestStatusChangeByAsync((e => e.DocumentId == documentId), userId);

            return new(latestStatus);
        }

        private async Task<DocumentAnalysisStatusChangedEvent> GetLatestStatusChangeByAsync(
            Predicate<DocumentAnalysisStatusChangedEvent> selector, Guid userId)
        {
            var analyses = await _facade.GetAllUserDocumentAnalysesAsync(userId);
            DocumentAnalysisStatusChangedEvent GetInitialEvent(Guid docId) =>
                analyses.FirstOrDefault(e => e.Status == OperationStatus.NotStarted && e.DocumentId == docId);
            var analysisStatusChanges = analyses.Where(ev => selector(ev));

            return analysisStatusChanges
                .OrderByDescending(s => s.Status)
                .Select(a => a with { DocumentName = GetInitialEvent(a.DocumentId)?.DocumentName ?? "No info"})
                .First()
                ;
        }
    }
}