namespace Yuki.Cmd.Actions.CreateTimeEntry
{
    using System;
    using PowerArgs;

    public class Args
    {
        [ArgDescription("Start time of the entry")]
        [ArgRequired]
        [ArgPosition(1)]
        public DateTime Start { get; set; }

        [ArgDescription("Duration of the entry")]
        [ArgRequired]
        [ArgPosition(2)]
        public int Duration { get; set; }

        [ArgDescription("Description for the time entry")]
        [ArgRequired]
        [ArgPosition(3)]
        public string Description { get; set; }

        [ArgDescription("Workspace id")]
        public int? Wid { get; set; }

        [ArgDescription("Project id")]
        public int? Pid { get; set; }

        [ArgDescription("Task id")]
        public int? Tid { get; set; }
    }
}
