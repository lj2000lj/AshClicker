namespace AshClicker;

using System;
using System.Runtime.InteropServices;
using System.Threading;

static class AshStopwatch
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool SetWaitableTimer(IntPtr hTimer, ref long pDueTime, int lPeriod, IntPtr pfnCompletionRoutine,
        IntPtr lpArgToCompletionRoutine, bool fResume);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CancelWaitableTimer(IntPtr hTimer);

    [DllImport("kernel32.dll", SetLastError = true)]
    static extern bool CloseHandle(IntPtr hObject);

    public static bool UsePrecise;

    public static void PreciseDelay(int milliseconds)
    {
        if (!UsePrecise)
        {
            Thread.Sleep(milliseconds);
            return;
        }

        IntPtr timer = CreateWaitableTimer(IntPtr.Zero, true, null);
        if (timer == IntPtr.Zero)
        {
            throw new InvalidOperationException("Failed to create timer");
        }

        try
        {
            long dueTime = -milliseconds * 10000;
            if (!SetWaitableTimer(timer, ref dueTime, 0, IntPtr.Zero, IntPtr.Zero, false))
            {
                throw new InvalidOperationException("Failed to set timer");
            }

            using var waitHandle = new AutoResetEvent(false);
            waitHandle.SafeWaitHandle = new Microsoft.Win32.SafeHandles.SafeWaitHandle(timer, ownsHandle: false);
            waitHandle.WaitOne();
        }
        finally
        {
            CancelWaitableTimer(timer);
            CloseHandle(timer);
        }
    }
}