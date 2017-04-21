namespace Yuki.Cmd.Actions
{
    using System;
    using System.Linq;
    using Newtonsoft.Json;
    using Yuki.Model;

    public class GetWorkspacesAction : IAction<GetWorkspacesArgs>
    {
        private readonly Repository repository;

        public GetWorkspacesAction(Repository repository)
        {
            this.repository = repository;
        }

        public void Execute(GetWorkspacesArgs args)
        {
            var workspaces = this.repository.GetAllWorkspaces();
            var model = workspaces.Select(x => new
            {
                x.Id,
                x.Name,
                Projects = x.Projects.Select(y => new { y.Id, y.Name }),
                Timers = x.Timers.Select(y => new { y.Id, y.Description, y.Started }),
            });

            var json = JsonConvert.SerializeObject(model, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
