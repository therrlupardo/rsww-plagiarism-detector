using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventsFacade;
using Queries;
using Queries.Enums;
using QueryService.Mock;

namespace QueryService.Services
{
    public class SourceService : ISourceService
    {
        private readonly SourceDocumentFacade _sourceDocumentFacade;

        public SourceService(SourceDocumentFacade sourceDocumentFacade)
        {
            _sourceDocumentFacade = sourceDocumentFacade;
        }

        public async Task<IEnumerable<SourceFile>> GetAllSourceFilesAsync()
        {
            var documents = await _sourceDocumentFacade.GetDocumentAddedToSourceEvents();

            return documents.Select(d =>
                new SourceFile(d.FileId, d.UserId, d.FileName, OperationStatus.Complete, d.OccurenceDate)).ToList();
        }

        public SourceFile GetById(Guid id)
        {
            return MockReadDataSource.GetSourceById(id);
        }
    }
}