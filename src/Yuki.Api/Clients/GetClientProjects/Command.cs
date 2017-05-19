namespace Yuki.Api.Clients.GetClientProjects
{
    using System;
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
                return None<Response, Exception>(
                    new NotImplementedException());
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}