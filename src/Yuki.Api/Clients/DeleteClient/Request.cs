﻿namespace Yuki.Api.Clients.DeleteClient
{
    public class Request
    {
        public Request(int clientId)
        {
            this.ClientId = clientId;
        }

        public int ClientId
        {
            get;
            private set;
        }
    }
}