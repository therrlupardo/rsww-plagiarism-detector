using System;
using System.Collections.Generic;
using Queries;
using QueryService.Mock;

namespace QueryService.Services
{
    public class SourceService : ISourceService
    {
        public List<SourceFile> GetAllSourceFiles()
        {
            return MockReadDataSource.GetAllSources();
        }

        public SourceFile GetById(Guid id)
        {
            return MockReadDataSource.GetSourceById(id);
        }
    }
}