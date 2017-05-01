namespace Yuki.Api.Projects
{
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, Project>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src["name"]))
                .ForMember(dest => dest.WorkspaceId, opts => opts.PreCondition(src => src.ContainsKey("wid")))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src["wid"]))
                .ForMember(dest => dest.ClientId, opts => opts.PreCondition(src => src.ContainsKey("cid")))
                .ForMember(dest => dest.ClientId, opts => opts.MapFrom(src => src["cid"]));
        }

        private static IDictionary<string, object> GetData(Project project)
        {
            var data = new Dictionary<string, object>
            {
                ["id"] = project.Id,
                ["wid"] = project.WorkspaceId,
                ["name"] = project.Name,
                ["active"] = project.Active,
                ["is_private"] = project.IsPrivate,
                ["at"] = project.LastUpdated.ToString(),
            };

            if (project.ClientId.HasValue)
            {
                data["cid"] = project.ClientId.Value;
            }

            return data;
        }
    }
}