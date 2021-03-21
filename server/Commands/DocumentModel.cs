using System.Collections.Generic;

namespace Commands
{
    public record FileModel (string FileName, IEnumerable<byte> Content)
    {
    }
}