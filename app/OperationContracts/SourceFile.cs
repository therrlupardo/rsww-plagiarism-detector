using System;
using OperationContracts.Enums;

namespace OperationContracts
{
    public record SourceFile (
        Guid Id,
        Guid UserId,
        string FileName,
        OperationStatus Status,
        DateTime Date
    );
}