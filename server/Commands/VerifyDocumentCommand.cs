using System;

namespace Commands
{
    public record VerifyDocumentCommand(Guid Id, Guid UserId, FileModel FileToVerify) : BaseCommand(Id, UserId)
    {
    }
}