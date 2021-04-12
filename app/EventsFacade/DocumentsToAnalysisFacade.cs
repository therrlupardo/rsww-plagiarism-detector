using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Commands;
using EventsFacade.Events;
using EventsFacade.Services;

namespace EventsFacade
{
    public class DocumentsToAnalysisFacade
    {
        private readonly IDocumentsToAnalysisService _documentsToAnalysisService;

        public DocumentsToAnalysisFacade(IDocumentsToAnalysisService documentsToAnalysisService)
        {
            _documentsToAnalysisService = documentsToAnalysisService;
        }

        public async Task SaveDocumentToAnalysisAddedAsync(AddDocumentToAnalysisCommand command)
        {
            var @event = new DocumentToAnalysisAddedEvent
            {
                FileName = command.FileToVerify.FileName,
                OccurenceDate = command.IssuedOn,
                FileId = command.TaskId,
                UserId = command.UserId
            };

            await _documentsToAnalysisService.SaveDocumentToAnalysisAddedEvent(@event);
        }

        public async Task<List<DocumentToAnalysisAddedEvent>> GetAllUserDocumentsToAnalysis(Guid userId) =>
            await _documentsToAnalysisService.GetAllUserDocumentsAddedToAnalysis(userId);
    }
}