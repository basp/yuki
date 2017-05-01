namespace Yuki.Api.Clients
{
    using Yuki.Data;

    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            this.CreateMap<ClientData, Client>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src.Wid));

            this.CreateMap<Client, ClientData>()
                .ForMember(dest => dest.Wid, opts => opts.MapFrom(src => src.WorkspaceId))
                .ForMember(dest => dest.At, opts => opts.MapFrom(src => src.LastUpdated.ToString()));
        }
    }
}