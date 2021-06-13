using System;
using OperationContracts.Enums;

namespace OperationContracts
{
    public record AnalysisFile(
        Guid DocumentId,
        Guid UserId,
        OperationStatus Status,
        string FileName,
        DateTime Date,
        double Result = 0.0
    )
    {
    }

}