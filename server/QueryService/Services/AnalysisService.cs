using System;
using System.Collections.Generic;
using Queries;
using QueryService.Mock;

namespace QueryService.Services
{
    public class AnalysisService : IAnalysisService
    {
        public List<AnalysisFile> GetAllAnalysis(Guid userId)
        {
            return MockReadDataSource.GetAllAnalysisByUserId(userId);
        }

        public AnalysisFile GetById(Guid analysisId, Guid userId)
        {
            return MockReadDataSource.GetById(analysisId, userId);
        }
    }
}