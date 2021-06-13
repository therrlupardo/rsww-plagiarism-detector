using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QueryService.Dto;
using QueryService.Services.Implementations;

namespace QueryService.Services
{
    public interface IAnalysisService
    {
        Task<List<AnalysisStatusDto>> GetUserAnalysesAsync(Guid userId);
        Task<AnalysisStatusDto> GetByDocumentIdAsync(Guid userId, Guid documentId);
        Task<AnalysisStatusDto> GetByIdAsync(Guid userId, Guid taskId);
    }
}