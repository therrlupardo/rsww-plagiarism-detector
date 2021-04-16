using System;

namespace EventsFacade.Utilities
{
    public static class EventStreamNamesUtils
    {
        public const string AnalysisStreamPrefix = "Analysis-";
        public const string DocumentsToAnalysisPrefix = "DocumentToAnalysis-";
        internal static string ToUserDocumentsToAnalysisStreamName(this Guid userId)
        {
            return $"{DocumentsToAnalysisPrefix}{userId}";
        }

        internal static string ToUserAnalysesStreamName(this Guid userId)
        {
            return $"{AnalysisStreamPrefix}{userId}";
        }
    }
}