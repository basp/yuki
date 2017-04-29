﻿namespace Yuki.Api.Clients.UpdateClient
{
    public class Response
    {
        public Response(ClientData data)
        {
            this.Data = data;
        }

        public ClientData Data
        {
            get;
            private set;
        }
    }
}