namespace Yuki.Api.Tags
{
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, Tag>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src["id"]))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src["name"]))
                .ForMember(dest => dest.WorkspaceId, opts => opts.PreCondition(src => src.ContainsKey("wid")))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src["wid"]));

            this.CreateMap<Tag, IDictionary<string, object>>()
                .ConstructUsing(GetData);
        }

        private static IDictionary<string, object> GetData(Tag tag)
        {
            var data = new Dictionary<string, object>
            {
                ["id"] = tag.Id,
                ["name"] = tag.Name,
                ["wid"] = tag.WorkspaceId,
            };

            return data;
        }
    }
}