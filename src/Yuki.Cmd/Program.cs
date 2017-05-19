namespace Yuki.Cmd
{
    using System;
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
        [ArgDescription("Get the current running timer (if any)")]
        public static void GetCurrent(Actions.GetCurrent.Args args) =>
            Container.GetInstance<Actions.GetCurrent.Action>()
                .Execute(args)
                .Wait();

        private static void Main(string[] args)
        {
            Container.Register(() => new TokenClient(
                Config.Server,
                "yukictl",
                "secret"));

            Args.InvokeAction<Program>(args);
        }
    }
}
