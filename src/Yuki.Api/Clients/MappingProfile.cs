namespace Yuki.Api.Clients
{
    using System.Collections.Generic;
    using Yuki.Data;

    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            this.CreateMap<IDictionary<string, object>, Client>()
                .ForMember(
                    dest => dest.Id, 
                    opts => opts.Ignore())
                .ForMember(
                    dest => dest.Name, 
                    opts => opts.MapFrom(src => src[F.Name]))
                .ForMember(
                    dest => dest.WorkspaceId, 
                    opts => opts.Condition(src => src.ContainsKey(F.Wid)))
                .ForMember(
                    dest => dest.WorkspaceId, 
                    opts => opts.Condition(
                        (src, dest) => dest.WorkspaceId == 0))
                .ForMember(
                    dest => dest.WorkspaceId, 
                    opts => opts.MapFrom(src => src[F.Wid]));

            this.CreateMap<Client, IDictionary<string, object>>()
                .ConstructUsing(GetData);
        }

        private static IDictionary<string, object> GetData(Client client)
        {
            var data = new Dictionary<string, object>
            {
                [F.Id] = client.Id,
                [F.Name] = client.Name,
                [F.Wid] = client.WorkspaceId,
                [F.At] = client.LastUpdated.ToString(),
            };

            return data;
        }
    }
}