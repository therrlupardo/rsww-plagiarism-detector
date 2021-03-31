using System;
using System.Collections.Generic;
using Queries;

namespace QueryService.Services
{
    public interface IAnalysisService
    {
        List<AnalysisFile> GetAllAnalysis(Guid userId);

        AnalysisFile GetById(Guid analysisId, Guid userId);
    }
}