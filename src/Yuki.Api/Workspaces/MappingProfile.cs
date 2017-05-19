namespace Yuki.Api.Workspaces
{
    using System;
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<Workspace, IDictionary<string, object>>()
                .ConstructUsing(GetData);
        }

        private static IDictionary<string, object> GetData(Workspace workspace)
        {
            var data = new Dictionary<string, object>
            {
                ["id"] = workspace.Id,
                ["name"] = workspace.Name,
            };

            return data;
        }
    }
}