using System.Collections.Generic;

namespace Commands
{
    public record FileModel (string FileName, byte[] Content)
    {
    }
}