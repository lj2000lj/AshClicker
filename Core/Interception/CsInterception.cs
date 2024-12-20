namespace AshClicker
{
    using System;
    using System.Runtime.InteropServices;

    class CsInterception
    {
        private const string DevicePath = "\\\\.\\interception00";

        // IOCTL Codes
        private const int METHOD_BUFFERED = 0;
        private const int FILE_DEVICE_UNKNOWN = 0x22;
        private const int FILE_ANY_ACCESS = 0;

        private static uint CTL_CODE(uint deviceType, uint function, uint method, uint access)
        {
            return (deviceType << 16) | (access << 14) | (function << 2) | method;
        }

        private static readonly uint IOCTL_SET_PRECEDENCE =
            CTL_CODE(FILE_DEVICE_UNKNOWN, 0x801, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static readonly uint IOCTL_GET_PRECEDENCE =
            CTL_CODE(FILE_DEVICE_UNKNOWN, 0x802, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static readonly uint IOCTL_SET_FILTER =
            CTL_CODE(FILE_DEVICE_UNKNOWN, 0x804, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static readonly uint IOCTL_GET_FILTER =
            CTL_CODE(FILE_DEVICE_UNKNOWN, 0x808, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static readonly uint IOCTL_WRITE =
            CTL_CODE(FILE_DEVICE_UNKNOWN, 0x820, METHOD_BUFFERED, FILE_ANY_ACCESS);

        private static readonly uint
            IOCTL_READ = CTL_CODE(FILE_DEVICE_UNKNOWN, 0x840, METHOD_BUFFERED, FILE_ANY_ACCESS);

        // Interception Constants
        public const int INTERCEPTION_MAX_KEYBOARD = 10;
        public const int INTERCEPTION_MAX_MOUSE = 10;
        public const int INTERCEPTION_MAX_DEVICE = INTERCEPTION_MAX_KEYBOARD + INTERCEPTION_MAX_MOUSE;

        public static int INTERCEPTION_KEYBOARD(int index) => index + 1;
        public static int INTERCEPTION_MOUSE(int index) => INTERCEPTION_MAX_KEYBOARD + index + 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct InterceptionMouseStroke
        {
            public ushort State;
            public ushort Flags;
            public short Rolling;
            public int X;
            public int Y;
            public uint Information;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct InterceptionKeyStroke
        {
            public ushort Code;
            public ushort State;
            public uint Information;
        }

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
        private static extern bool DeviceIoControl(
            IntPtr hDevice,
            uint dwIoControlCode,
            IntPtr lpInBuffer,
            uint nInBufferSize,
            IntPtr lpOutBuffer,
            uint nOutBufferSize,
            out uint lpBytesReturned,
            IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        private IntPtr deviceHandle;

        public bool OpenDevice()
        {
            deviceHandle = CreateFile(DevicePath, 0xC0000000, 0, IntPtr.Zero, 3, 0, IntPtr.Zero);
            return deviceHandle != IntPtr.Zero;
        }

        public bool CloseDevice()
        {
            if (deviceHandle != IntPtr.Zero)
            {
                bool result = CloseHandle(deviceHandle);
                deviceHandle = IntPtr.Zero;
                return result;
            }

            return false;
        }

        public bool SendKeyStroke(InterceptionKeyStroke keyStroke)
        {
            if (deviceHandle == IntPtr.Zero) return false;

            IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InterceptionKeyStroke)));
            Marshal.StructureToPtr(keyStroke, buffer, false);

            bool success = DeviceIoControl(
                deviceHandle,
                IOCTL_WRITE,
                buffer,
                (uint)Marshal.SizeOf(typeof(InterceptionKeyStroke)),
                IntPtr.Zero,
                0,
                out _,
                IntPtr.Zero
            );

            Marshal.FreeHGlobal(buffer);
            return success;
        }

        public bool SendMouseStroke(InterceptionMouseStroke mouseStroke)
        {
            if (deviceHandle == IntPtr.Zero) return false;

            IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(InterceptionMouseStroke)));
            Marshal.StructureToPtr(mouseStroke, buffer, false);

            bool success = DeviceIoControl(
                deviceHandle,
                IOCTL_WRITE,
                buffer,
                (uint)Marshal.SizeOf(typeof(InterceptionMouseStroke)),
                IntPtr.Zero,
                0,
                out _,
                IntPtr.Zero
            );

            Marshal.FreeHGlobal(buffer);
            return success;
        }

        public static void Main(string[] args)
        {
            var interception = new CsInterception();

            if (!interception.OpenDevice())
            {
                Console.WriteLine("Failed to open interception device.");
                return;
            }

            Console.WriteLine("Device opened successfully.");

            var keyStroke = new InterceptionKeyStroke
            {
                Code = 0x1E, // Example key code for 'A'
                State = 0x00, // Key down
                Information = 0
            };

            if (interception.SendKeyStroke(keyStroke))
            {
                Console.WriteLine("Key stroke sent successfully.");
            }
            else
            {
                Console.WriteLine("Failed to send key stroke.");
            }

            interception.CloseDevice();
        }
    }
}