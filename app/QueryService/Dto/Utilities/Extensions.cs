using System;
using OperationContracts;
using QueryService.Services.Implementations;

namespace QueryService.Dto.Utilities
{
    public static class Extensions
    {
        public static AnalysisFile ToAnalysisFile(this AnalysisStatusDto dto, Guid userId)
        {
            return new(
                Date: dto.LatestChangeDate,
                DocumentId: dto.DocumentId,
                FileName: dto.DocumentName,
                Status: dto.Status,
                UserId: userId
            );
        }
    }
}