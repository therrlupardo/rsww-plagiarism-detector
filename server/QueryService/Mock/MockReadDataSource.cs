using System;
using System.Collections.Generic;
using System.Linq;
using Queries;
using Queries.Enums;

namespace QueryService.Mock
{
    public static class MockReadDataSource
    {
        private static readonly List<AnalysisFile> _analysisFiles = new()
        {
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Running,
                "running.py",
                DateTime.Now,
                null
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Waiting,
                "waiting.py",
                DateTime.Now,
                null
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Complete,
                "complete.py",
                DateTime.Now,
                "10%"
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Complete,
                "complete2.py",
                DateTime.Now,
                "100%"
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.NewGuid(),
                OperationStatus.Complete,
                "analysis_done_by_other_user__should_be_skipped_in_results.py",
                DateTime.Now,
                "100%"
            )
        };

        private static List<SourceFile> _sourceFiles = new()
        {
            new SourceFile(Guid.NewGuid(),
                Guid.NewGuid(),
                "source1.py",
                OperationStatus.Complete,
                DateTime.Now
            ),
            new SourceFile(Guid.NewGuid(),
                Guid.NewGuid(),
                "source2.py",
                OperationStatus.Waiting,
                DateTime.Now
            ),
            new SourceFile(Guid.NewGuid(),
                Guid.NewGuid(),
                "source3.py",
                OperationStatus.Running,
                DateTime.Now
            )
        };


        public static List<AnalysisFile> GetAllAnalysisByUserId(Guid userId)
        {
            return _analysisFiles.FindAll(file => file.UserId.Equals(userId));
        }

        public static AnalysisFile GetById(Guid id, Guid userId)
        {
            return _analysisFiles.First(file => file.Id.Equals(id) && file.UserId.Equals(userId));
        }
    }
}