using System.Collections.Generic;

namespace Commands
{
    public record FileModel
    {
        public FileModel(
            string fileName,
            IEnumerable<byte> content
            ) { }
    }
}