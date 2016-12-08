namespace Yuki
{
    using PowerArgs;

    public class SessionArgs
    {
        [ArgDescription("The server to connect to")]
        [ArgShortcut("s")]
        [ArgRequired]
        [ArgPosition(1)]
        public string Server { get; set; }
    }
}
