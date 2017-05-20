namespace Yuki.Cmd.Actions.StopTimeEntry
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        [ArgDescription("Time entry id")]
        public int Id { get; set; }
    }
}
