using System;
using Queries;

namespace QueryService.Dto
{
    public record SourceFileResponse (Guid Id, Guid UserId, string FileName, string Status, DateTime Date)
    {
        public SourceFileResponse(SourceFile file) : this(
            file.Id,
            file.UserId,
            file.FileName,
            Enum.GetName(file.Status),
            file.Date)
        {
        }
    }
}