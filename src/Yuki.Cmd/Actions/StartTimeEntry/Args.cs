namespace Yuki.Cmd.Actions.StartTimeEntry
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgDescription("Workspace id")]
        public int Wid { get; set; }

        [ArgDefaultValue("No description")]
        [ArgDescription("Time entry description")]
        public string Description { get; set; }
    }
}
