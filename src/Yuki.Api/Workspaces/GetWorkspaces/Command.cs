namespace Yuki.Api.Workspaces.GetWorkspaces
{
    using System;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly Repository<Workspace> workspaceRepository;

        public Command(
            Repository<Workspace> workspaceRepository)
        {
            this.workspaceRepository = workspaceRepository;
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