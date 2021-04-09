using System;

namespace Commands
{
    public record VerifyDocumentCommand(Guid Id, Guid UserId, FileModel FileToVerify, DateTime IssuedOn) : BaseCommand(Id, UserId, IssuedOn)
    {
    }
}