using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade;

namespace QueryService.Services.Implementations
{
    internal class DocumentsToAnalysisService : IDocumentsToAnalysisService
    {
        private readonly DocumentsToAnalysisFacade _facade;

        public DocumentsToAnalysisService(DocumentsToAnalysisFacade facade)
        {
            _facade = facade;
        }

        public async Task<IEnumerable<(string fileName, Guid fileId)>> GetDocumentsToAnalysis(Guid userId)
        {
            var documentAddedEvents = await _facade.GetAllUserDocumentsToAnalysis(userId);

            return documentAddedEvents.Select(e => (e.FileName, e.FileId));
        }
    }
}