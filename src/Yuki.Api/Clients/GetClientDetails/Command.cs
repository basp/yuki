namespace Yuki.Api.Clients.GetClientDetails
{
    using System;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;
    using AutoMapper;

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