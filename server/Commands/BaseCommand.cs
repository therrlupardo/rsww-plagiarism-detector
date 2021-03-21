namespace Commands
{
    public record BaseCommand
    {
        protected BaseCommand(
            Guid id,
            Guid userId
        )
        {
        }
    }
}