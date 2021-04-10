using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CommandService.Services
{
    public interface IAnalysisService
    {
        Task<Guid> SendFileToAnalysis(IFormFile file, Guid userId);

        Task<Guid> AnalyzeFile(Guid fileId, Guid userId);
    }
}