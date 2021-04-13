using System;

namespace OperationContracts
{
    public record AddDocumentToAnalysisCommand(Guid FileId, Guid UserId, FileModel FileToVerify, DateTime IssuedOn) : BaseCommand(FileId, UserId, IssuedOn);
}