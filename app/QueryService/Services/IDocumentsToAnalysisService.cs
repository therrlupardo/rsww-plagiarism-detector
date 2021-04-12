using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueryService.Services
{
    public interface IDocumentsToAnalysisService
    {
        Task<IEnumerable<(string fileName, Guid fileId)>> GetDocumentsToAnalysis(Guid userId);
    }
}