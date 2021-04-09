using System;

namespace EventsFacade.Utilities
{
    public static class EventStreamGuidExtensions
    {
        internal static string ToUserSourceDocumentsEventsStreamIdentifier(this Guid userId)
        {
            return $"SourceDocuments-{userId}";
        }
    }
}