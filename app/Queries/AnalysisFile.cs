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
        double Result = 0
    )
    {
    }
}