using System;

namespace EventsFacade.Utilities
{
    public static class EventStreamGuidExtensions
    {
        private const string Analysis = "Analysis";
        private const string DocumentsToAnalysis = "DocumentToAnalysis";
        internal static string ToUserDocumentsToAnalysisStreamName(this Guid userId)
        {
            return $"{DocumentsToAnalysis}-{userId}";
        }

        internal static string ToUserAnalysisStreamName(this Guid userId)
        {
            return $"{Analysis}-{userId}";
        }
    }
}