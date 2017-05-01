namespace Yuki.Api.Clients.GetClientDetails
{
    using System;
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