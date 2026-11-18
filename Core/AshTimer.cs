using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AshClicker
{
    public sealed class AshTimer
    {
        public static readonly AshTimer Instance = new AshTimer();

        private readonly object _syncRoot = new object();
        private Task _clickerTask;

        public Action Func { get; set; } = () => { };
        public int Delay { get; set; } = 1;

        private volatile bool _enabled;

        public bool Enabled => _enabled;

        private AshTimer()
        {
        }

        [DllImport("winmm.dll")]
        private static extern uint timeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll")]
        private static extern uint timeEndPeriod(uint uMilliseconds);

        public void Start()
        {
            lock (_syncRoot)
            {
                if (_clickerTask != null && !_clickerTask.IsCompleted)
                {
                    return;
                }

                _enabled = true;
                timeBeginPeriod(1);
                _clickerTask = Task.Run(() =>
                {
                    while (Enabled)
                    {
                        Func.Invoke();
                        AshStopwatch.PreciseDelay(Delay);
                    }
                });
            }
        }

        public void Stop()
        {
            Task taskToWait;
            lock (_syncRoot)
            {
                if (!Enabled && _clickerTask == null) return;

                _enabled = false;
                taskToWait = _clickerTask;
                _clickerTask = null;
            }

            if (taskToWait != null)
            {
                try
                {
                    taskToWait.Wait();
                }
                catch (AggregateException)
                {
                }
            }

            timeEndPeriod(1);
        }
    }
}
