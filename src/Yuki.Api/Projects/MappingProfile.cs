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
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src[F.Name]))
                .ForMember(dest => dest.WorkspaceId, opts => opts.PreCondition(src => src.ContainsKey(F.Wid)))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src[F.Wid]))
                .ForMember(dest => dest.ClientId, opts => opts.PreCondition(src => src.ContainsKey(F.Cid)))
                .ForMember(dest => dest.ClientId, opts => opts.MapFrom(src => src[F.Cid]));
        }

        private static IDictionary<string, object> GetData(Project project)
        {
            var data = new Dictionary<string, object>
            {
                [F.Id] = project.Id,
                [F.Wid] = project.WorkspaceId,
                [F.Name] = project.Name,
                [F.Active] = project.Active,
                [F.IsPrivate] = project.IsPrivate,
                [F.At] = project.LastUpdated.ToString(),
            };

            if (project.ClientId.HasValue)
            {
                data[F.Cid] = project.ClientId.Value;
            }

            return data;
        }
    }
}