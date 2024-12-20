namespace AshClicker
{
    using System;
    using System.Runtime.InteropServices;

    public static class NtKeyboardSimulator
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint Type;
            public InputUnion Data;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KEYBDINPUT ki;
            [FieldOffset(0)] public HardwareInput hi;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_SCANCODE = 0x0008;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        // NtUserSendInput is an undocumented Windows API
        [DllImport("win32u.dll", EntryPoint = "NtUserSendInput", SetLastError = true)]
        private static extern uint NtUserSendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        public static void SimulateKeyPress(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[2];

            // Key down event
            inputs[0] = new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        Vk = 0,
                        Scan = scanCode,
                        Flags = KEYEVENTF_SCANCODE,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            // Key up event
            inputs[1] = new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        Vk = 0,
                        Scan = scanCode,
                        Flags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            if (NtUserSendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                Console.WriteLine("NtUserSendInput failed with error code: " + Marshal.GetLastWin32Error());
            }
        }

        public static void SimulateKeyDown(ushort scanCode)
        {
            INPUT input = new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        Vk = 0,
                        Scan = scanCode,
                        Flags = KEYEVENTF_SCANCODE,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            if (NtUserSendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                Console.WriteLine("Key down failed with error code: " + Marshal.GetLastWin32Error());
            }
        }

        public static void SimulateKeyUp(ushort scanCode)
        {
            INPUT input = new INPUT
            {
                Type = INPUT_KEYBOARD,
                Data = new InputUnion
                {
                    ki = new KEYBDINPUT
                    {
                        Vk = 0,
                        Scan = scanCode,
                        Flags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP,
                        Time = 0,
                        ExtraInfo = IntPtr.Zero
                    }
                }
            };

            if (NtUserSendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT))) == 0)
            {
                Console.WriteLine("Key up failed with error code: " + Marshal.GetLastWin32Error());
            }
        }
    }
}