namespace Yuki.Api.Tags
{
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, Tag>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src[F.Name]))
                .ForMember(dest => dest.WorkspaceId, opts => opts.PreCondition(src => src.ContainsKey(F.Wid)))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src[F.Wid]));

            this.CreateMap<Tag, IDictionary<string, object>>()
                .ConstructUsing(GetData);
        }

        private static IDictionary<string, object> GetData(Tag tag)
        {
            var data = new Dictionary<string, object>
            {
                [F.Id] = tag.Id,
                [F.Name] = tag.Name,
                [F.Wid] = tag.WorkspaceId,
            };

            return data;
        }
    }
}