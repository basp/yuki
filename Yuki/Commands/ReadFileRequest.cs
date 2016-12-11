namespace Yuki.Commands
{
    using PowerArgs;

    public class ReadFileRequest
    {
        [ArgRequired]
        [ArgPosition(1)]
        public string Path { get; set; }
    }
}
