using System.Collections.Generic;

namespace OperationContracts
{
    public record FileModel (string FileName, IEnumerable<byte> Content)
    {
    }
}