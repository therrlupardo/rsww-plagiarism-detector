using System;
using Queries;

namespace QueryService.Dto
{
    public record AnalysisFileResponse(
        Guid Id,
        Guid UserId,
        string FileName,
        string Status,
        DateTime Date,
        string Result)
    {
        public AnalysisFileResponse(AnalysisFile file) : this(
            file.Id,
            file.UserId,
            file.FileName,
            Enum.GetName(file.Status),
            file.Date,
            file.Result)
        {
        }
    }
}