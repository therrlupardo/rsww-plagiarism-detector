using System;

namespace Commands
{
    public record AnalyzeDocumentCommand(Guid TaskId, Guid FileId, Guid UserId, DateTime IssuedTime)
    {
        
    }
}