using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AshClicker
{
    public class InterceptionControl
    {
        // 临时 DLL 文件路径
        private static string tempDllPath;

        // DLL Handle
        private static IntPtr dllHandle = IntPtr.Zero;

        // Delegate definitions for Interception functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr InterceptionCreateContextDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void InterceptionDestroyContextDelegate(IntPtr context);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InterceptionReceiveDelegate(IntPtr context, int device, IntPtr stroke, uint timeout);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InterceptionSendDelegate(IntPtr context, int device, IntPtr stroke, uint numStrokes);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InterceptionIsKeyboardDelegate(int device);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int InterceptionIsMouseDelegate(int device);

        // Interception function pointers
        private static InterceptionCreateContextDelegate interception_create_context;
        private static InterceptionDestroyContextDelegate interception_destroy_context;
        private static InterceptionReceiveDelegate interception_receive;
        private static InterceptionSendDelegate interception_send;
        private static InterceptionIsKeyboardDelegate interception_is_keyboard;
        private static InterceptionIsMouseDelegate interception_is_mouse;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeLibrary(IntPtr hModule);

        // Context for Interception
        private IntPtr _context;

        public InterceptionControl()
        {
            tempDllPath = TempDllManager.ExtractTempDll();
            dllHandle = LoadLibrary(tempDllPath);
            if (dllHandle == IntPtr.Zero)
            {
                throw new InvalidOperationException($"Failed to load DLL. Error: {Marshal.GetLastWin32Error()}");
            }

            interception_create_context =
                GetFunctionDelegate<InterceptionCreateContextDelegate>("interception_create_context");
            interception_destroy_context =
                GetFunctionDelegate<InterceptionDestroyContextDelegate>("interception_destroy_context");
            interception_receive = GetFunctionDelegate<InterceptionReceiveDelegate>("interception_receive");
            interception_send = GetFunctionDelegate<InterceptionSendDelegate>("interception_send");
            interception_is_keyboard = GetFunctionDelegate<InterceptionIsKeyboardDelegate>("interception_is_keyboard");
            interception_is_mouse = GetFunctionDelegate<InterceptionIsMouseDelegate>("interception_is_mouse");

            _context = interception_create_context();
            if (_context == IntPtr.Zero)
            {
                throw new Exception("Failed to create Interception context.");
            }
            else
            {
                Console.WriteLine("Interception loaded successfully.");
            }
        }

        public static void Cleanup()
        {
            // 释放 DLL
            if (dllHandle != IntPtr.Zero)
            {
                FreeLibrary(dllHandle);
                dllHandle = IntPtr.Zero;
            }

            // 删除临时 DLL 文件
            if (!string.IsNullOrEmpty(tempDllPath))
            {
                TempDllManager.CleanupTempDll(tempDllPath);
                tempDllPath = null;
            }
        }

        private static T GetFunctionDelegate<T>(string functionName) where T : Delegate
        {
            IntPtr procAddress = GetProcAddress(dllHandle, functionName);
            if (procAddress == IntPtr.Zero)
            {
                throw new InvalidOperationException(
                    $"Failed to get address of function '{functionName}'. Error: {Marshal.GetLastWin32Error()}");
            }

            return Marshal.GetDelegateForFunctionPointer<T>(procAddress);
        }

        ~InterceptionControl()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_context != IntPtr.Zero)
            {
                interception_destroy_context(_context);
                _context = IntPtr.Zero;
            }

            Cleanup();
        }

        public bool IsKeyboard(int device)
        {
            return interception_is_keyboard(device) != 0;
        }

        public bool IsMouse(int device)
        {
            return interception_is_mouse(device) != 0;
        }

        public int Receive(int device, ref Stroke stroke, uint timeout = 1000)
        {
            IntPtr strokePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Stroke)));
            try
            {
                int result = interception_receive(_context, device, strokePtr, timeout);
                if (result > 0)
                {
                    stroke = Marshal.PtrToStructure<Stroke>(strokePtr);
                }

                return result;
            }
            finally
            {
                Marshal.FreeHGlobal(strokePtr);
            }
        }

        public int Send(int device, ref Stroke stroke)
        {
            IntPtr strokePtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(Stroke)));
            try
            {
                Marshal.StructureToPtr(stroke, strokePtr, false);
                return interception_send(_context, device, strokePtr, 1);
            }
            finally
            {
                Marshal.FreeHGlobal(strokePtr);
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Stroke
        {
            [FieldOffset(0)] public KeyStroke Key;

            [FieldOffset(0)] public MouseStroke Mouse;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KeyStroke
        {
            public ushort Code;
            public ushort State;
            public uint Information;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseStroke
        {
            public ushort Flags;
            public ushort Rolling;
            public int X;
            public int Y;
            public uint Information;
        }
    }
}