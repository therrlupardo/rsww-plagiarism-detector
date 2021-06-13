using System;
using OperationContracts.Enums;

namespace EventsFacade.Events
{
    public record DocumentAnalysisStatusChangedEvent : BaseEvent
    {
        public Guid TaskId { get; init; }
        public Guid DocumentId { get; init; }
        public OperationStatus Status { get; init; }
        public string DocumentName { get; init; }
        public double Result { get; init; }
    }
}