namespace AshClicker
{
    public class AshKey
    {
        public string KeyName { get; }
        public int VKey { get; }
        public int DdCode { get; }
        public int ScanCode { get; }
        public string Description { get; }

        public AshKey(string keyName, int vKey, int ddCode, int scanCode, string description)
        {
            KeyName = keyName;
            VKey = vKey;
            DdCode = ddCode;
            ScanCode = scanCode;
            Description = description;
        }
    }
}