using System;
using Queries.Enums;

namespace Queries
{
    public record AnalysisFile(
        Guid Id,
        Guid UserId,
        OperationStatus Status,
        string FileName,
        DateTime Date,
        // FIXME: maybe it should be some numeric type, depends on what will analysis algorithm return 
        string Result
    )
    {
    }
}