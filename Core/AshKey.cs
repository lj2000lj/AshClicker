using System.Runtime.InteropServices;

namespace AshClicker
{
    public class AshKey
    {
        public string KeyName { get; }
        public int VKey { get; }
        public int DdCode { get; }
        public int ScanCode { get; }
        public string Description { get; }

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public AshKey(string keyName, int vKey, int ddCode, string description)
        {
            KeyName = keyName;
            VKey = vKey;
            DdCode = ddCode;
            ScanCode = (int)MapVirtualKey((uint)vKey, 0);
            Description = description;
        }
    }
}