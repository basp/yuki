﻿namespace Yuki.Api.Clients.UpdateClient
{
    using System;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly Repository repository;

        public Command(Repository repository)
        {
            this.repository = repository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                var client = this.repository.GetClientById(req.ClientId);
                Mapper.Map(req.Client, client);
                this.repository.UpdateClient(client);

                var data = Mapper.Map<ClientData>(client);
                var res = new Response(data);

                return Some<Response, Exception>(res);
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}