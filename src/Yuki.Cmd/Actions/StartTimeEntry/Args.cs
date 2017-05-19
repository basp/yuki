﻿namespace Yuki.Cmd.Actions.StartTimeEntry
{
    using PowerArgs;

    public class Args
    {
        [ArgRequired]
        [ArgPosition(1)]
        public int UserId { get; set; }

        [ArgRequired]
        [ArgPosition(2)]
        public int WorkspaceId { get; set; }
    }
}