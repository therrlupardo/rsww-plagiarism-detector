using System;
using Queries.Enums;

namespace EventsFacade.Events
{
    public record DocumentAnalysisStatusChangedEvent : BaseEvent
    {
        public Guid TaskId { get; set; }
        public Guid FileId { get; set; }
        public OperationStatus Status { get; set; }
    }
}