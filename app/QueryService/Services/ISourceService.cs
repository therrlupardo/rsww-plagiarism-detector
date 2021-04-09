using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Queries;

namespace QueryService.Services
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceFile>> GetAllSourceFilesAsync();

        SourceFile GetById(Guid id);
    }
}