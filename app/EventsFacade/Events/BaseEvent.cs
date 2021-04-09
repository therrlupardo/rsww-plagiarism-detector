using System;

namespace EventsFacade
{
    public abstract record BaseEvent
    {
        public DateTime OccurenceDate { get; init; }
    }
}