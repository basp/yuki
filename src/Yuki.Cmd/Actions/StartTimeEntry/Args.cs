namespace Yuki.Cmd.Actions.StartTimeEntry
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        public int WorkspaceId { get; set; }

        [ArgDefaultValue("No description")]
        public string Description { get; set; }
    }
}
