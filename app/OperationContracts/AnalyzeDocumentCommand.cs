using System;

namespace OperationContracts
{
    public record AnalyzeDocumentCommand(Guid TaskId, Guid FileId, Guid UserId, DateTime IssuedTime) : BaseCommand(TaskId, UserId, IssuedTime);
}