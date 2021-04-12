using System;

namespace Commands
{
    public record AddDocumentToAnalysisCommand(Guid TaskId, Guid UserId, FileModel FileToVerify, DateTime IssuedOn) : BaseCommand(TaskId, UserId, IssuedOn);
}