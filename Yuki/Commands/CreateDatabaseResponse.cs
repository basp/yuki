namespace Yuki.Commands
{
    public class CreateDatabaseResponse
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public bool Created { get; set; }
    }
}
