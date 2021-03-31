using System.ComponentModel;

namespace Queries.Enums
{
    public enum OperationStatus
    {
        [Description("WAITING")] Waiting,
        [Description("RUNNING")] Running,
        [Description("COMPLETE")] Complete
    }
}