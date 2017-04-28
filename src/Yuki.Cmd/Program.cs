namespace Yuki.Cmd
{
    using IdentityModel.Client;
    using PowerArgs;
    using SimpleInjector;
    using Yuki.Cmd.Actions;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class Program
    {
        private static readonly Container Container = new Container();

        [HelpHook]
        public static bool Help { get; set; }

        [ArgActionMethod]
        public static void GetWorkspaces(GetWorkspacesArgs args) => 
            Container.GetInstance<GetWorkspacesAction>()
                .Execute(args)
                .Wait();

        private static void Main(string[] args)
        {
            Container.Register(() => new TokenClient(
                "http://localhost:5000/connect/token",
                "carbon",
                "21B5F798-BE55-42BC-8AA8-0025B903DC3B"));

            Args.InvokeAction<Program>(args);
        }
    }
}
