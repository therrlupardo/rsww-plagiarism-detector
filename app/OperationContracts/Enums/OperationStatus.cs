using System.ComponentModel;

namespace OperationContracts.Enums
{
    public enum OperationStatus
    {
        [Description("NOT INITIALIZED")] NotInitialized,
        [Description("NOT STARTED")] NotStarted,
        [Description("WAITING")] Waiting,
        [Description("RUNNING")] Running,
        [Description("COMPLETE")] Complete,
        [Description("FAILED")] Failed
    }
}