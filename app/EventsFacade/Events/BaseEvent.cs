using System;

namespace EventsFacade.Events
{
    public abstract record BaseEvent
    {
        public DateTime OccurenceDate { get; init; }
    }
}