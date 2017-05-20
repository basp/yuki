namespace Yuki.Cmd.Actions.GetSingleWorkspace
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgDescription("Workspace id")]
        public int Id { get; set; }
    }
}
