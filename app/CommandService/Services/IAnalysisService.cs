using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CommandService.Services
{
    public interface IAnalysisService
    {
        Task<Guid> PerformAnalysis(IFormFile file, Guid userId);
    }
}