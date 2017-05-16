namespace Yuki.Api.Clients.CreateClient
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
                var client = Mapper.Map<Client>(req.Client);
                this.repository.Insert(client);

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