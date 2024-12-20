using System;
using System.Runtime.InteropServices;

namespace AshClicker
{
    public class InterceptionClient : IDisposable
    {
        private const string DevicePath = "\\\\.\\interception00";

        private const uint GENERIC_READ = 0x80000000;
        private const uint GENERIC_WRITE = 0x40000000;
        private const uint OPEN_EXISTING = 3;

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        private IntPtr deviceHandle;

        public InterceptionClient()
        {
            deviceHandle = CreateFile(DevicePath, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0,
                IntPtr.Zero);
            if (deviceHandle == IntPtr.Zero)
            {
                throw new Exception("Failed to open interception device.");
            }
        }

        public void SendKeyStroke(KeyboardInputData keyStroke)
        {
            SendInput(keyStroke);
        }

        public void SendMouseStroke(ushort state, ushort flags, short rolling, int x, int y)
        {
            var mouseStroke = new InterceptionMouseStroke
            {
                State = state,
                Flags = flags,
                Rolling = rolling,
                X = x,
                Y = y,
                Information = 0
            };

            SendInput(mouseStroke);
        }

        private void SendInput<T>(T stroke) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);

            try
            {
                Marshal.StructureToPtr(stroke, buffer, false);

                if (!DeviceIoControl(deviceHandle, InterceptionConstants.IOCTL_WRITE, buffer, (uint)size, IntPtr.Zero,
                        0, out _, IntPtr.Zero))
                {
                    throw new Exception("Failed to send input.");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }

        public void Dispose()
        {
            if (deviceHandle != IntPtr.Zero)
            {
                CloseHandle(deviceHandle);
                deviceHandle = IntPtr.Zero;
            }
        }
    }

    public static class InterceptionConstants
    {
        private const int METHOD_BUFFERED = 0;
        private const int FILE_DEVICE_UNKNOWN = 0x22;
        private const int FILE_ANY_ACCESS = 0;

        public static uint IOCTL_WRITE => CTL_CODE(FILE_DEVICE_UNKNOWN, 0x820, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static uint CTL_CODE(uint deviceType, uint function, uint method, uint access)
        {
            return (deviceType << 16) | (access << 14) | (function << 2) | method;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct KeyboardInputData
    {
        public ushort UnitId; // 2 bytes
        public ushort MakeCode; // 2 bytes
        public ushort Flags; // 2 bytes
        public ushort Reserved; // 2 bytes
        public uint ExtraInformation; // 4 bytes
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InterceptionMouseStroke
    {
        public ushort State;
        public ushort Flags;
        public short Rolling;
        public int X;
        public int Y;
        public uint Information;
    }

    public static class DataMapper
    {
        public static KeyboardInputData MapToKeyboardInputData(InterceptionKeyStroke keyStroke)
        {
            return new KeyboardInputData
            {
                UnitId = 0,
                MakeCode = keyStroke.Code,
                Flags = keyStroke.State,
                Reserved = 0,
                ExtraInformation = keyStroke.Information
            };
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct InterceptionKeyStroke
    {
        public ushort Code;
        public ushort State;
        public uint Information;
    }
}