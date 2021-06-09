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
        string Result = "0.0"
    )
    {
    }

}