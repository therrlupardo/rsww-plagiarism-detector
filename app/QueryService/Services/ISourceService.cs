using System;
using System.Collections.Generic;
using Queries;

namespace QueryService.Services
{
    public interface ISourceService
    {
        List<SourceFile> GetAllSourceFiles();

        SourceFile GetById(Guid id);
    }
}