using System;

namespace EventsFacade.Events
{
    public record DocumentToAnalysisAddedEvent : BaseEvent
    {
        public Guid FileId { get; init; }
        public Guid UserId { get; init; }
        public string FileName { get; init; }
    }
}