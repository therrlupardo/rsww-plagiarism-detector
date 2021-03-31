using System;

namespace Commands
{
    public record AddDocumentToSourceStoreCommand(Guid Id, Guid UserId, FileModel File) : BaseCommand(Id, UserId)
    {
    }
}