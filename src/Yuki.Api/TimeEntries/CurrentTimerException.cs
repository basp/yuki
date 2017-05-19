namespace Yuki.Api.TimeEntries
{
    using System;

    public class CurrentTimerException : InvalidOperationException
    {
        public CurrentTimerException()
            : base("There's already a timer running.")
        {
        }
    }
}