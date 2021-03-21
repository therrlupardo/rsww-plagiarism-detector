namespace Commands
{
    public record VerifyDocumentCommand : BaseCommand
    {
        public VerifyDocumentCommand(
            Guid id,
            Guid userId,
            FileModel fileToVerify
        ) : base(id, userId)
        {
        }
    }
}