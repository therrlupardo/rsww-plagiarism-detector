namespace Commands
{
    public record AddDocumentToSourceStoreCommand : BaseCommand
    {
        public AddDocumentToSourceStoreCommand(
            Guid id,
            Guid userId,
            FileModel file
        ) : base(id, userId)
        {
        }
    }
}