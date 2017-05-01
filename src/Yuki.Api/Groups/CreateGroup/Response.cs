namespace Yuki.Api.Groups.CreateGroup
{
    using System.Collections.Generic;

    public class Response
    {
        public Response(IDictionary<string, object> data)
        {
            this.Data = data;
        }

        public IDictionary<string, object> Data
        {
            get;
            private set;
        }
    }
}