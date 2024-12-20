namespace AshClicker
{
    public class AshKeypress
    {
        public AshKey UserPress { get; }
        public AshKey WillPress { get; }
        public int Mode { get; }

        public AshKeypress(AshKey userPress, AshKey willPress, int mode)
        {
            UserPress = userPress;
            WillPress = willPress;
            Mode = mode;
        }
    }
}