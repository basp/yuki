namespace Yuki.Api.Workspaces.GetWorkspaces
{
    using System.Collections.Generic;

    public class Response
    {
        public Response(IEnumerable<IDictionary<string,object>> items)
        {
            this.Items = items;
        }

        public IEnumerable<IDictionary<string, object>> Items
        {
            get;
            private set;
        }
    }
}