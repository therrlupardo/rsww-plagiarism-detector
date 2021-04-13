using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OperationContracts;

namespace QueryService.Services
{
    public interface ISourceService
    {
        Task<IEnumerable<SourceFile>> GetAllSourceFilesAsync();

        // Dunno if this will get an actual implementation
        SourceFile GetById(Guid id);
    }
}