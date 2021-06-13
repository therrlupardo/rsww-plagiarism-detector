using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade;
using OperationContracts;
using QueryService.Mock;

namespace QueryService.Services.Implementations
{
    internal class SourceService : ISourceService
    {
        private readonly SourceDocumentFacade _sourceDocumentFacade;


        public SourceService(SourceDocumentFacade sourceDocumentFacade)
        {
            _sourceDocumentFacade = sourceDocumentFacade;
        }

        public async Task<IEnumerable<SourceFile>> GetAllSourceFilesAsync()
        {
            var documents = await _sourceDocumentFacade.GetDocumentAddedToSourceEvents();

            return documents
                .GroupBy(d => d.FileId)
                .Select(docs => docs.OrderByDescending(d => d.OccurenceDate).First())
                .Select(d =>
                new SourceFile(d.FileId, d.UserId, d.FileName, d.Status, d.OccurenceDate)).ToList();
        }

        public SourceFile GetById(Guid id)
        {
            return MockReadDataSource.GetSourceById(id);
        }
    }
}