using System;
using OperationContracts.Enums;

namespace QueryService.Dto
{
    public record DocumentToAnalysisResponse (Guid Id, string Name, DateTime AddedDate, string status);
}