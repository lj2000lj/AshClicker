namespace AshClicker;

using System;
using System.Runtime.InteropServices;

public static class KeyboardSimulator
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

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

    /// <summary>
    /// Simulates a key press using the specified scan code.
    /// </summary>
    /// <param name="scanCode">The scan code of the key to simulate.</param>
    public static void SimulateKeyPress(ushort scanCode)
    {
        // Create an array of two inputs: key down and key up
        INPUT[] inputs = new INPUT[2];

        // Key down event
        inputs[0] = new INPUT
        {
            Type = INPUT_KEYBOARD,
            Data = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    Vk = 0, // Virtual-Key code (not used for scancode input)
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

        // Send the input events
        if (SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT))) == 0)
        {
            Console.WriteLine("SendInput failed with error code: " + Marshal.GetLastWin32Error());
        }
    }
    public static void SimulateKeyDown(ushort scanCode)
    {
        // Create the key down input
        INPUT input = new INPUT
        {
            Type = INPUT_KEYBOARD,
            Data = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    Vk = 0, // Virtual-Key code (not used for scancode input)
                    Scan = scanCode,
                    Flags = KEYEVENTF_SCANCODE,
                    Time = 0,
                    ExtraInfo = IntPtr.Zero
                }
            }
        };

        // Send the key down input
        if (SendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT))) == 0)
        {
            Console.WriteLine("Key down failed with error code: " + Marshal.GetLastWin32Error());
        }
    }

    public static void SimulateKeyUp(ushort scanCode)
    {
        // Create the key up input
        INPUT input = new INPUT
        {
            Type = INPUT_KEYBOARD,
            Data = new InputUnion
            {
                ki = new KEYBDINPUT
                {
                    Vk = 0, // Virtual-Key code (not used for scancode input)
                    Scan = scanCode,
                    Flags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP,
                    Time = 0,
                    ExtraInfo = IntPtr.Zero
                }
            }
        };

        // Send the key up input
        if (SendInput(1, new INPUT[] { input }, Marshal.SizeOf(typeof(INPUT))) == 0)
        {
            Console.WriteLine("Key up failed with error code: " + Marshal.GetLastWin32Error());
        }
    }

}
