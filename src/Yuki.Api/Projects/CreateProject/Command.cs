namespace Yuki.Api.Projects.CreateProject
{
    using System;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly Repository<Project> repository;

        public Command(Repository<Project> repository)
        {
            this.repository = repository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                return None<Response, Exception>(new Exception());
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}