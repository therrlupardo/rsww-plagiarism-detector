using System;

namespace OperationContracts
{
    public record AddDocumentToSourceStoreCommand(Guid FileId, Guid UserId,  DateTime IssuedOn, FileModel File)
     : BaseCommand(FileId, UserId, IssuedOn);
}