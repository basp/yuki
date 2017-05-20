namespace Yuki.Cmd
{
    using System;
    using IdentityModel.Client;
    using PowerArgs;
    using Serilog;
    using SimpleInjector;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class Program
    {
        private static readonly Container Container = new Container();

        [HelpHook]
        public static bool Help { get; set; }

        [ArgActionMethod]
        [ArgDescription("Get the user workspaces")]
        public static void GetWorkspaces(Actions.GetWorkspaces.Args args) =>
            Container.GetInstance<Actions.GetWorkspaces.Action>()
                .Execute(args)
                .Wait();

        [ArgActionMethod]
        [ArgDescription("Get a single workspace")]
        public static void GetSingleWorkspace(
            Actions.GetSingleWorkspace.Args args) =>
                Container.GetInstance<Actions.GetSingleWorkspace.Action>()
                    .Execute(args)
                    .Wait();

        [ArgActionMethod]
        [ArgDescription("Get the current running timer (if any)")]
        public static void GetCurrent(Actions.GetCurrent.Args args) =>
            Container.GetInstance<Actions.GetCurrent.Action>()
                .Execute(args)
                .Wait();

        [ArgActionMethod]
        [ArgDescription("Starts a new time entry")]
        public static void StartTimeEntry(Actions.StartTimeEntry.Args args) =>
            Container.GetInstance<Actions.StartTimeEntry.Action>()
                .Execute(args)
                .Wait();

        [ArgActionMethod]
        [ArgDescription("Stops a running time entry")]
        public static void StopTimeEntry(Actions.StopTimeEntry.Args args) =>
            Container.GetInstance<Actions.StopTimeEntry.Action>()
                .Execute(args)
                .Wait();

        [ArgActionMethod]
        [ArgDescription("Create a new time entry")]
        public static void CreateTimeEntry(Actions.CreateTimeEntry.Args args) =>
            Container.GetInstance<Actions.CreateTimeEntry.Action>()
                .Execute(args)
                .Wait();

        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.LiterateConsole()
                .CreateLogger();

            Container.Register(() => new TokenClient(
                Config.TokenEndPoint,
                Config.ClientId,
                Config.ClientSecret));

            try
            {
                Args.InvokeAction<Program>(args);
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "Failed to execute action: {ErrorMessage}",
                    ex.Message);
            }
        }
    }
}
