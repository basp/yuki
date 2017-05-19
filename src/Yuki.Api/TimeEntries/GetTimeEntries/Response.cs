namespace Yuki.Api.TimeEntries.GetTimeEntries
{
    using System.Collections.Generic;

    public class Response
    {
        public IEnumerable<IDictionary<string,object>> Items { get; set; }
    }
}