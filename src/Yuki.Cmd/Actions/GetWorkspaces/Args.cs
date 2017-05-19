namespace Yuki.Cmd.Actions.GetWorkspaces
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        public int UserId { get; set; }
    }
}
