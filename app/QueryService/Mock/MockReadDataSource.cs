using System;
using System.Collections.Generic;
using System.Linq;
using OperationContracts;
using OperationContracts.Enums;

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
                DateTime.Now
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Waiting,
                "waiting.py",
                DateTime.Now
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Complete,
                "complete.py",
                DateTime.Now,
                "10"
            ),
            new AnalysisFile(
                Guid.Parse("a5b5a7ee-6406-4579-abc4-5f4bac38b0a2"),
                Guid.Parse("a81bc81b-dead-4e5d-abff-90865d1e13b1"),
                OperationStatus.Complete,
                "complete2.py",
                DateTime.Now,
                "30"
            ),
            new AnalysisFile(
                Guid.NewGuid(),
                Guid.NewGuid(),
                OperationStatus.Complete,
                "analysis_done_by_other_user__should_be_skipped_in_results.py",
                DateTime.Now,
                "100"
            )
        };

        private static readonly List<SourceFile> _sourceFiles = new()
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

        public static AnalysisFile GetAnalysisById(Guid id, Guid userId)
        {
            return _analysisFiles.First(file => file.DocumentId.Equals(id) && file.UserId.Equals(userId));
        }

        public static List<SourceFile> GetAllSources()
        {
            return _sourceFiles;
        }

        public static SourceFile GetSourceById(Guid id)
        {
            return _sourceFiles.First(file => file.Id.Equals(id));
        }
    }
}