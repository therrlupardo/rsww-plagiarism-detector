using System;

namespace EventsFacade.Events
{
    public record DocumentToAnalysisAddedEvent : BaseEvent
    {
        public Guid TaskId { get; init; }
        public Guid UserId { get; init; }
        public string FileName { get; init; }
    }
}