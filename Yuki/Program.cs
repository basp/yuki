namespace Yuki
{
    using System;
    using System.ComponentModel;
    using Actions;
    using Newtonsoft.Json;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using PowerArgs;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class Program
    {
        private const string DefaultLoggingLayout = @"${pad:padding=5:inner=${level:uppercase=true}} ${date:format=HH\:mm\:ss} ${logger} ${message}";

        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        [HelpHook]
        [ArgDescription("Display help")]
        public bool Help { get; set; }

        [ArgActionMethod]
        [ArgDescription("Validate a Yuki project folder")]
        public void Info(InfoArgs args)
        {
            var ctx = Context.GetCurrent();
            var action = new InfoAction(ctx);
            var result = action.Execute(args);
            var json = JsonConvert.SerializeObject(
                result.Value,
                Formatting.Indented);

            Console.WriteLine(json);
        }

        [ArgActionMethod]
        [ArgDescription("Create a new Yuki project folder")]
        [ArgExample(
            @"yuki init",
            "Initialize a Yuki project in the current working directory")]
        [ArgExample(
            @"yuki init -f .\frotz",
            "Initialize a project in the frotz folder")]
        [ArgExample(
            @"yuki init -force -overwrite",
            "Force init in a folder that is not empty, overwriting any existing files")]
        [DisplayName("init")]
        public void Init(InitArgs args)
        {
            var action = new InitAction();
            action.Execute(args);
        }

        [ArgActionMethod]
        [ArgDescription("Scaffold a Yuki project")]
        [ArgExample(
            @"yuki scaffold",
            "Scaffold a Yuki project")]
        [ArgExample(
            @"yuki scaffold -cfg alternate-config.json",
            "Specify an alternate configuration file")]
        [ArgExample(
            @"yuki scaffold -force -overwrite",
            "Force scaffold in a folder that is not empty, overwriting any existing files")]
        [DisplayName("scaffold")]
        public void Scaffold(ScaffoldArgs args)
        {
            var ctx = Context.GetCurrent();
            var action = new ScaffoldAction(ctx);
            action.Execute(args);
        }

        [ArgActionMethod]
        [ArgDescription("Create a new database")]
        [DisplayName("create-database")]
        public void CreateDatabase(CreateDatabaseArgs args)
        {
            var ctx = Context.GetCurrent();
            using (var session = args.Server.Connect())
            {
                var action = new CreateDatabaseAction(session);
                var result = action.Execute(args);
                if (result.IsError)
                {
                    throw result.Exception;
                }
            }
        }

        [ArgActionMethod]
        [ArgDescription("Setup databases")]
        [DisplayName("setup")]
        public void Setup(SetupArgs args)
        {
            var ctx = Context.GetCurrent();
            var cs = $"Server=ROSPC0297\\SQLEXPRESS;Integrated Security=SSPI";
            var migrator = new Migrator(args.Folder);
            using (var session = SqlSession.Create(cs))
            {
                session.Connect();
                var action = new SetupAction(ctx, session, migrator);
                var result = action.Execute(args);
                if (result.IsError)
                {
                    throw result.Exception;
                }
            }
        }

        [ArgActionMethod]
        [ArgDescription("Migrate a SQL Server instance")]
        [ArgExample(
            @"yuki migrate -s SQL\INSTANCE -r $/proj",
            "Basic usage")]
        [ArgExample(
            @"yuki migrate -s SQL\INSTANCE -r $/project -ts foo=bar, quux=frotz",
            "Suplly a list of tokens to be used during token replacement")]
        [DisplayName("migrate")]
        public void Migrate(MigrateArgs args)
        {
            var ctx = Context.GetCurrent();

            var msgs = new[]
            {
                $"Using config file at {ctx.ProjectFile}",
                $"Connecting with database server instance {args.Server}",
                $"Looking in {ctx.ProjectDirectory} for scripts to run"
            };

            Array.ForEach(msgs, this.log.Info);

            var action = new MigrateAction(ctx);
            action.Execute(args);
        }

        private static void InitializeLogManager()
        {
            var loggingConfig = new LoggingConfiguration();

            var target = new ColoredConsoleTarget()
            {
                Name = "console",
                Layout = DefaultLoggingLayout
            };

            var rule = new LoggingRule("*", LogLevel.Debug, target);

            loggingConfig.AddTarget(target);
            loggingConfig.LoggingRules.Add(rule);

            LogManager.Configuration = loggingConfig;
        }

        private static void Main(string[] args)
        {
            InitializeLogManager();

            try
            {
                // TODO: I wanna `new`-up `Program` myself.
                Args.InvokeAction<Program>(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}