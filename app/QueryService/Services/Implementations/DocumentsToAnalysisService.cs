using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade;
using QueryService.Dto;

namespace QueryService.Services.Implementations
{
    internal class DocumentsToAnalysisService : IDocumentsToAnalysisService
    {
        private readonly DocumentsToAnalysisFacade _facade;

        public DocumentsToAnalysisService(DocumentsToAnalysisFacade facade)
        {
            _facade = facade;
        }

        public async Task<IEnumerable<DocumentToAnalysisResponse>> GetDocumentsToAnalysis(Guid userId)
        {
            var documentAddedEvents = await _facade.GetAllUserDocumentsToAnalysis(userId);

            return documentAddedEvents.Select(e => new DocumentToAnalysisResponse(e.FileId, e.FileName));
        }
    }
}