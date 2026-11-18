using System.Collections.Generic;
using System.Linq;

namespace AshClicker
{
    public enum AshTriggerMode
    {
        Hold = 0,
        Toggle = 1
    }

    public sealed class AshKeyMapping
    {
        public int TriggerKeyCode { get; set; }
        public AshTriggerMode TriggerMode { get; set; }
        public List<AshSequenceStep> Steps { get; set; } = new List<AshSequenceStep>();

        public AshKeyMapping Clone()
        {
            return new AshKeyMapping
            {
                TriggerKeyCode = TriggerKeyCode,
                TriggerMode = TriggerMode,
                Steps = Steps.Select(step => step.Clone()).ToList()
            };
        }
    }

    public sealed class AshSequenceStep
    {
        public int KeyCode { get; set; }
        public int RepeatCount { get; set; } = 1;
        public int HoldMilliseconds { get; set; }
        public int DelayAfterMilliseconds { get; set; }

        public AshSequenceStep Clone()
        {
            return new AshSequenceStep
            {
                KeyCode = KeyCode,
                RepeatCount = RepeatCount,
                HoldMilliseconds = HoldMilliseconds,
                DelayAfterMilliseconds = DelayAfterMilliseconds
            };
        }
    }
}
