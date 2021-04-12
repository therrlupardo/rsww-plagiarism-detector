using System;
using System.Collections.Generic;
using Queries;
using QueryService.Mock;

namespace QueryService.Services.Implementations
{
    internal class AnalysisService : IAnalysisService
    {
        public List<AnalysisFile> GetAllAnalysis(Guid userId)
        {
            return MockReadDataSource.GetAllAnalysisByUserId(userId);
        }

        public AnalysisFile GetById(Guid analysisId, Guid userId)
        {
            return MockReadDataSource.GetAnalysisById(analysisId, userId);
        }
    }
}