using System;

namespace Commands
{
    public record AddDocumentToSourceStoreCommand(Guid Id, Guid UserId,  DateTime IssuedOn, FileModel File)
     : BaseCommand(Id, UserId, IssuedOn);
}