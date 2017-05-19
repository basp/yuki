namespace Yuki.Cmd.Actions.GetSingleWorkspace
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        public int WorkspaceId { get; set; }
    }
}
