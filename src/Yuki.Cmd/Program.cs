namespace Yuki.Cmd
{
    using IdentityModel.Client;
    using PowerArgs;
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

        private static void Main(string[] args)
        {
            Container.Register(() => new TokenClient(
                Config.TokenEndPoint,
                Config.ClientId,
                Config.ClientSecret));

            Args.InvokeAction<Program>(args);
        }
    }
}
