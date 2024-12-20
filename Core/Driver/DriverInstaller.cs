using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace AshClicker.Driver
{
    class DriverInstaller
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint GetSystemDirectory([Out] char[] lpBuffer, uint uSize);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegCreateKeyEx(
            UIntPtr hKey,
            string lpSubKey,
            uint Reserved,
            string lpClass,
            uint dwOptions,
            uint samDesired,
            IntPtr lpSecurityAttributes,
            out UIntPtr phkResult,
            out uint lpdwDisposition);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegSetValueEx(
            UIntPtr hKey,
            string lpValueName,
            int Reserved,
            RegistryValueKind dwType,
            byte[] lpData,
            uint cbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegQueryValueEx(
            UIntPtr hKey,
            string lpValueName,
            IntPtr lpReserved,
            out uint lpType,
            byte[] lpData,
            ref uint lpcbData);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegCloseKey(UIntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int RegOpenKeyEx(
            UIntPtr hKey,
            string lpSubKey,
            uint ulOptions,
            uint samDesired,
            out UIntPtr phkResult);

        private static readonly UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;

        public static bool InstallDriver()
        {
            // Check if drivers or registry keys already exist
            if (DriverOrRegistryExists())
            {
                Console.WriteLine("Driver or registry keys already exist. Installation aborted.");
                return false;
            }

            // Step 1: Get system directory
            char[] systemDirectory = new char[260];
            GetSystemDirectory(systemDirectory, 260);
            string systemPath = new string(systemDirectory).TrimEnd('\0');

            // Step 2: Extract driver files
            string keyboardDriverPath = Path.Combine(systemPath, @"drivers\keyboard.sys");
            string mouseDriverPath = Path.Combine(systemPath, @"drivers\mouse.sys");

            File.WriteAllBytes(keyboardDriverPath, Properties.Resources.keyboard); // 从资源提取文件
            File.WriteAllBytes(mouseDriverPath, Properties.Resources.mouse);

            // Step 3: Configure keyboard driver in registry
            ConfigureDriverRegistry("keyboard", "Keyboard Upper Filter Driver", keyboardDriverPath);

            // Step 4: Configure mouse driver in registry
            ConfigureDriverRegistry("mouse", "Mouse Upper Filter Driver", mouseDriverPath);

            // Step 5: Set UpperFilters for keyboard
            SetUpperFilters(
                @"System\CurrentControlSet\Control\Class\{4D36E96B-E325-11CE-BFC1-08002BE10318}",
                "keyboard"
            );

            // Step 6: Set UpperFilters for mouse
            SetUpperFilters(
                @"System\CurrentControlSet\Control\Class\{4D36E96F-E325-11CE-BFC1-08002BE10318}",
                "mouse"
            );

            Console.WriteLine("Driver installation completed successfully.");
            return true;
        }

        private static bool DriverOrRegistryExists()
        {
            // Check if driver files exist
            string systemPath = new string(new char[260]);
            GetSystemDirectory(systemPath.ToCharArray(), 260);
            systemPath = systemPath.TrimEnd('\0');

            string keyboardDriverPath = Path.Combine(systemPath, @"drivers\keyboard.sys");
            string mouseDriverPath = Path.Combine(systemPath, @"drivers\mouse.sys");

            if (File.Exists(keyboardDriverPath) || File.Exists(mouseDriverPath))
            {
                return true;
            }

            // Check if registry keys exist
            if (RegistryKeyExists(@"System\CurrentControlSet\Services\keyboard") ||
                RegistryKeyExists(@"System\CurrentControlSet\Services\mouse") ||
                RegistryKeyExists(@"System\CurrentControlSet\Control\Class\{4D36E96B-E325-11CE-BFC1-08002BE10318}") ||
                RegistryKeyExists(@"System\CurrentControlSet\Control\Class\{4D36E96F-E325-11CE-BFC1-08002BE10318}"))
            {
                return true;
            }

            return false;
        }

        private static bool RegistryKeyExists(string path)
        {
            UIntPtr key;
            int result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, path, 0, 0x20019, out key);
            if (result == 0)
            {
                RegCloseKey(key);
                return true;
            }

            return false;
        }

        private static void ConfigureDriverRegistry(string serviceName, string displayName, string driverPath)
        {
            UIntPtr key;
            uint disposition;

            string registryPath = $@"System\CurrentControlSet\Services\{serviceName}";

            // Create registry key
            int result = RegCreateKeyEx(
                HKEY_LOCAL_MACHINE,
                registryPath,
                0,
                null,
                0,
                0x20019, // KEY_WRITE
                IntPtr.Zero,
                out key,
                out disposition
            );

            if (result != 0)
            {
                throw new Exception($"Failed to create registry key for {serviceName}, Error: {result}");
            }

            // Set DisplayName
            byte[] displayNameBytes = System.Text.Encoding.ASCII.GetBytes(displayName);
            RegSetValueEx(key, "DisplayName", 0, RegistryValueKind.String, displayNameBytes,
                (uint)displayNameBytes.Length);

            // Set Type
            byte[] typeBytes = BitConverter.GetBytes(0x1);
            RegSetValueEx(key, "Type", 0, RegistryValueKind.DWord, typeBytes, 4);

            // Set ErrorControl
            byte[] errorControlBytes = BitConverter.GetBytes(0x1);
            RegSetValueEx(key, "ErrorControl", 0, RegistryValueKind.DWord, errorControlBytes, 4);

            // Set Start
            byte[] startBytes = BitConverter.GetBytes(0x3);
            RegSetValueEx(key, "Start", 0, RegistryValueKind.DWord, startBytes, 4);

            // Close the key
            RegCloseKey(key);
        }

        private static void SetUpperFilters(string classPath, string filterName)
        {
            UIntPtr key;
            uint type;
            uint dataSize = 0;

            // Open registry key
            int result = RegOpenKeyEx(HKEY_LOCAL_MACHINE, classPath, 0, 0x20019, out key);
            if (result != 0)
            {
                throw new Exception($"Failed to open registry key {classPath}, Error: {result}");
            }

            // Query current UpperFilters value
            result = RegQueryValueEx(key, "UpperFilters", IntPtr.Zero, out type, null, ref dataSize);
            byte[] data = new byte[dataSize + filterName.Length + 1]; // Allocate space for existing data + new filter
            if (result == 0)
            {
                RegQueryValueEx(key, "UpperFilters", IntPtr.Zero, out type, data, ref dataSize);
            }

            // Append new filter name
            string currentFilters = System.Text.Encoding.ASCII.GetString(data).TrimEnd('\0');
            string newFilters = currentFilters + (currentFilters.Length > 0 ? "\0" : "") + filterName + "\0";

            byte[] newFiltersBytes = System.Text.Encoding.ASCII.GetBytes(newFilters);

            // Write updated UpperFilters value
            RegSetValueEx(key, "UpperFilters", 0, RegistryValueKind.MultiString, newFiltersBytes,
                (uint)newFiltersBytes.Length);

            // Close the key
            RegCloseKey(key);
        }

        static void Main(string[] args)
        {
            try
            {
                InstallDriver();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}