namespace Yuki.Api.Clients.CreateClient
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
                var client = Mapper.Map<Client>(req.Client);
                this.repository.InsertClient(client);

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