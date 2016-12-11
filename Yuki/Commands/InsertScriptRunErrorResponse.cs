namespace Yuki.Commands
{
    public class InsertScriptRunErrorResponse : IRepositoryResponse
    {
        public string Server { get; set; }

        public string Database { get; set; }

        public string Schema { get; set; }
    }
}
