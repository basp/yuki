namespace Yuki.Api.Groups
{
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, Group>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src[F.Name]))
                .ForMember(dest => dest.WorkspaceId, opts => opts.PreCondition(src => src.ContainsKey(F.Wid)))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src[F.Wid]));
        }

        private static IDictionary<string, object> GetData(Group group)
        {
            var data = new Dictionary<string, object>
            {
                [F.Id] = group.Id,
                [F.Name] = group.Name,
                [F.Wid] = group.WorkspaceId,
                [F.At] = group.LastUpdated.ToString(),
            };

            return data;
        }
    }
}