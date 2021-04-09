using System;
using Queries.Enums;

namespace Queries
{
    public record SourceFile (
        Guid Id,
        Guid UserId,
        string FileName,
        OperationStatus Status,
        DateTime Date
    );
}