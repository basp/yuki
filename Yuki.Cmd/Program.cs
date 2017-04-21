namespace Yuki.Cmd
{
    using PowerArgs;
    using SimpleInjector;
    using Yuki.Cmd.Actions;
    using Yuki.Model;

    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    internal class Program
    {
        private static Container container = new Container();

        [HelpHook]
        public static bool Help { get; set; }
        
        [ArgActionMethod]
        public static void GetWorkspaces(GetWorkspacesArgs args) =>
            container.GetInstance<GetWorkspacesAction>().Execute(args);

        private static void Main(string[] args)
        {
            container.Register<DataContext>(Lifestyle.Singleton);
            Args.InvokeAction<Program>(args);
        }
    }
}
