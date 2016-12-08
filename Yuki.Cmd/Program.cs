namespace Yuki.Cmd
{
    using System;
    using System.IO;
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using PowerArgs;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    class Program
    {
        static void Main(string[] args)
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

        static void InitializeLogManager()
        {
            var loggingConfig = new LoggingConfiguration();

            var target = new ColoredConsoleTarget()
            {
                Name = "console",
                Layout = @"${date:format=HH\:mm\:ss} ${logger} ${message}"
            };

            var rule = new LoggingRule("*", NLog.LogLevel.Debug, target);

            loggingConfig.AddTarget(target);
            loggingConfig.LoggingRules.Add(rule);

            LogManager.Configuration = loggingConfig;
        }

        [HelpHook]
        [ArgDescription("Display help")]
        public bool Help { get; set; }

        //[ArgDescription("Set the log level for the default console target")]
        //[ArgDefaultValue("Info")]
        //public string LogLevel { get; set; }

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
        public void Scaffold(ScaffoldArgs args)
        {
            var ctx = Context.GetCurrent();
            var action = new ScaffoldAction(ctx);
            action.Execute(args);
        }

        public void Restore(RestoreArgs args)
        {
            var action = new RestoreAction();
            action.Execute(args);
        }

        [ArgActionMethod]
        [ArgDescription("Migrate a SQL Server instance")]
        [ArgExample(
            @"yuki migrate -s SQL\INSTANCE -r $/proj",
            "Basic usage")]
        [ArgExample(
            @"yuki migrate -s SQL\INSTANCE -r $/project -ts foo=bar, quux=frotz",
            "Suplly a list of tokens to be used during token replacement")]
        public void Migrate(MigrateArgs args)
        {
            var ctx = Context.GetCurrent();

            this.log.Info($"Using config file at {ctx.ProjectFile}");
            this.log.Info($"Connecting with database server instance {args.Server}");
            this.log.Info($"Looking in {ctx.ProjectDirectory} for scripts to run");

            var action = new MigrateAction(ctx);
            action.Execute(args);
        }

        readonly ILogger log = LogManager.GetCurrentClassLogger();
    }
}