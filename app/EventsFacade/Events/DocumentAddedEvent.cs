using System;

namespace EventsFacade.Events
{
    public record DocumentAddedEvent : BaseEvent
    {
        public Guid FileId { get; init; }

        public string FileName { get; init; }
        public Guid UserId { get; set; }
    }
}