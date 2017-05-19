namespace Yuki.Cmd.Actions.StopTimeEntry
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        public int TimeEntryId { get; set; }

    }
}
