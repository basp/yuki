namespace Yuki.Cmd.Actions.GetCurrent
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        public int UserId { get; set; }
    }
}
