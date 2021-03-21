namespace Commands
{
    public record FileModel
    {
        public FileModel(
            string fileName,
            IEnumerable<byte> content
        )
        {
        }
    }
}