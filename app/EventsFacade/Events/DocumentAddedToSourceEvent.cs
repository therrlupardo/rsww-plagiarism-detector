using System;
using OperationContracts.Enums;

namespace EventsFacade.Events
{
    public record DocumentAddedToSourceEvent : BaseEvent
    {
        public Guid FileId { get; init; }

        public string FileName { get; init; }
        public Guid UserId { get; init; }
        
        public OperationStatus Status { get; init; }
    }
}