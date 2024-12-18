using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AshClicker;

public class AshTimer
{
    public static readonly AshTimer Instance = new AshTimer();

    private Task? _clickerTask = null;
    private static readonly CancellationTokenSource Source = new CancellationTokenSource();
    private readonly object _lockObject = new object();

    public Action Func { get; set; } = () => { };
    public int Delay { get; set; } = 1;

    private bool _enabled = false;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            if (_enabled)
            {
                Start();
            }
            else
            {
                Stop();
            }
        }
    }

    private AshTimer()
    {
    }

    [DllImport("winmm.dll")]
    public static extern uint timeBeginPeriod(uint uMilliseconds);

    [DllImport("winmm.dll")]
    public static extern uint timeEndPeriod(uint uMilliseconds);

    public void Start()
    {
        if (!Enabled)
        {
            Enabled = true;
            return;
        }

        timeBeginPeriod(1);
        lock (_lockObject)
        {
            if (_clickerTask != null && !_clickerTask.IsCompleted)
            {
                MessageBox.Show("按键已经开启了");
                return;
            }

            _clickerTask = Task.Run(() =>
            {
                while (!Source.Token.IsCancellationRequested && Enabled)
                {
                    Func.Invoke();
                    AshStopwatch.PreciseDelay(Delay);
                }
            }, Source.Token);
        }
    }

    public void Stop()
    {
        if (Enabled)
        {
            Enabled = false;
            return;
        }

        timeEndPeriod(1);
        lock (_lockObject)
        {
            if (_clickerTask != null)
            {
                try
                {
                    _clickerTask.Wait();
                }
                catch (AggregateException)
                {
                }
                finally
                {
                    _clickerTask = null;
                }
            }
        }
    }
}