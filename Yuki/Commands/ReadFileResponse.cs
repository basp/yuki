namespace Yuki.Commands
{
    public class ReadFileResponse
    {
        public string Path { get; set; }

        public string FileName { get; set; }

        public string Contents { get; set; }

        public string Hash { get; set; }
    }
}
