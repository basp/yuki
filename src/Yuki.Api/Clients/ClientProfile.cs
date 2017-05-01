namespace Yuki.Api.Clients
{
    using AutoMapper;
    using Yuki.Data;

    public class ClientProfile : Profile
    {
        public ClientProfile()
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