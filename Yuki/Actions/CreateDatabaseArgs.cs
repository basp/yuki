namespace Yuki.Actions
{
    using PowerArgs;

    public class CreateDatabaseArgs
    {
        [ArgDescription("Name of the database to create")]
        [ArgShortcut("n")]
        [ArgRequired]
        [ArgPosition(1)]
        public string Name { get; set; }

        [ArgRequired]
        [ArgPosition(2)]
        public ISession Server { get; set; }

        [ArgReviver]
        public static ISession SessionReviver(string name, string value)
        {
            var cs = $"Server={value};Integrated Security=SSPI";
            return SqlSession.Create(cs);
        }
    }
}
