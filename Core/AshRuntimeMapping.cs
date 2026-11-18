using System.Collections.Generic;

namespace AshClicker
{
    internal sealed class AshRuntimeMapping
    {
        private int _stepIndex;
        private int _repeatIndex;

        public int TriggerKeyCode { get; set; }
        public AshTriggerMode TriggerMode { get; set; }
        public List<AshRuntimeStep> Steps { get; } = new List<AshRuntimeStep>();
        public int NextRunAt { get; private set; }
        public bool WasActive { get; private set; }

        public AshRuntimeStep CurrentStep => Steps[_stepIndex];

        public void SetActive(bool active, int now)
        {
            if (active && !WasActive)
            {
                _stepIndex = 0;
                _repeatIndex = 0;
                NextRunAt = now;
            }

            if (!active)
            {
                _stepIndex = 0;
                _repeatIndex = 0;
            }

            WasActive = active;
        }

        public void Advance(int nextRunAt)
        {
            _repeatIndex++;
            if (_repeatIndex >= CurrentStep.RepeatCount)
            {
                _repeatIndex = 0;
                _stepIndex = (_stepIndex + 1) % Steps.Count;
            }

            NextRunAt = nextRunAt;
        }
    }

    internal sealed class AshRuntimeStep
    {
        public int OutputCode { get; set; }
        public int RepeatCount { get; set; }
        public int HoldMilliseconds { get; set; }
        public int DelayAfterMilliseconds { get; set; }
    }
}
