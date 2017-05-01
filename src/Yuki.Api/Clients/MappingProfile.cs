namespace Yuki.Api.Clients
{
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, Client>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src["name"]))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition(src => src.ContainsKey("wid")))
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src["wid"]));

            this.CreateMap<Client, IDictionary<string, object>>()
                .ConstructUsing(GetData);
        }

        private static IDictionary<string, object> GetData(Client client)
        {
            var data = new Dictionary<string, object>
            {
                ["id"] = client.Id,
                ["name"] = client.Name,
                ["wid"] = client.WorkspaceId,
                ["at"] = client.LastUpdated.ToString(),
            };

            return data;
        }
    }
}