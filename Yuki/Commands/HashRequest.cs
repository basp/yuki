namespace Yuki.Commands
{
    using PowerArgs;

    public class HashRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        public string File
        {
            get;
            set;
        }
    }
}
