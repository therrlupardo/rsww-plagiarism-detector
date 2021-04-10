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
        private readonly DocumentsToAnalysisService _documentsToAnalysisService;

        public DocumentsToAnalysisFacade(DocumentsToAnalysisService documentsToAnalysisService)
        {
            _documentsToAnalysisService = documentsToAnalysisService;
        }

        public async Task SaveDocumentToAnalysisAdded(AddDocumentToAnalysisCommand command)
        {
            var @event = new DocumentToAnalysisAddedEvent
            {
                FileName = command.FileToVerify.FileName,
                OccurenceDate = command.IssuedOn,
                TaskId = command.TaskId,
                UserId = command.UserId
            };

            await _documentsToAnalysisService.SaveDocumentToAnalysisAddedEvent(@event);
        }

        public async Task<List<DocumentToAnalysisAddedEvent>> GetAllUserDocumentsToAnalysis(Guid userId) =>
            await _documentsToAnalysisService.GetAllUserDocumentsAddedToAnalysis(userId);
    }
}