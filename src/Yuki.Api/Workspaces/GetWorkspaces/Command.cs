namespace Yuki.Api.Workspaces.GetWorkspaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly WorkspaceRepository workspaceRepository;
        private readonly WorkspaceUserRepository workspaceUserRepository;

        public Command(
            WorkspaceRepository workspaceRepository,
            WorkspaceUserRepository workspaceUserRepository)
        {
            this.workspaceRepository = workspaceRepository;
            this.workspaceUserRepository = workspaceUserRepository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            { 
                var workspaceIds = this.workspaceUserRepository
                    .GetWorkspacesByUserId(req.UserId)
                    .Select(x => x.WorkspaceId)
                    .ToArray();

                var workspaces = this.workspaceRepository
                    .GetWorkspaces(workspaceIds);

                var items = Mapper.Map<IEnumerable<IDictionary<string, object>>>(workspaces);
                return Some<Response, Exception>(new Response(items));
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}