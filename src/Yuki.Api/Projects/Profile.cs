namespace Yuki.Api.Projects
{
    using Yuki.Data;

    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            this.CreateMap<ProjectData, Project>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.WorkspaceId, opts => opts.Condition((src, dest) => dest.WorkspaceId == 0))
                .ForMember(dest => dest.WorkspaceId, opts => opts.MapFrom(src => src.Wid))
                .ForMember(dest => dest.ClientId, opts => opts.MapFrom(src => src.Cid));

            this.CreateMap<Project, ProjectData>()
                .ForMember(dest => dest.Wid, opts => opts.MapFrom(src => src.WorkspaceId))
                .ForMember(dest => dest.Cid, opts => opts.MapFrom(src => src.ClientId))
                .ForMember(dest => dest.At, opts => opts.MapFrom(src => src.LastUpdated.ToString()));
        }
    }
}