using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QueryService.Dto;
using QueryService.Services.Implementations;

namespace QueryService.Services
{
    public interface IDocumentsToAnalysisService
    {
        Task<IEnumerable<DocumentToAnalysisResponse>> GetDocumentsToAnalysis(Guid userId);
    }
}