namespace Yuki.Api.Groups.DeleteGroup
{
    using System;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly Repository<Group> repository;

        public Command(Repository<Group> repository)
        {
            this.repository = repository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                return None<Response, Exception>(new NotImplementedException());
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}