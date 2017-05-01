namespace Yuki.Api.Clients.UpdateClient
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly Repository<Client> repository;

        public Command(Repository<Client> repository)
        {
            this.repository = repository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                var client = this.repository.GetById(req.ClientId);
                Mapper.Map(req.Client, client);
                this.repository.Update(client);

                var data = Mapper.Map<IDictionary<string, object>>(client);
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