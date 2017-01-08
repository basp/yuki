namespace Yuki
{
    using AutoMapper;
    using Commands;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<CreateDatabaseRequest, CreateDatabaseResponse>();

            this.CreateMap<GetCurrentHashRequest, GetCurrentHashResponse>();

            this.CreateMap<GetVersionRequest, GetVersionResponse>();

            this.CreateMap<HasScriptRunRequest, HasScriptRunResponse>();

            this.CreateMap<InitializeRepositoryRequest, InitializeRepositoryResponse>();

            this.CreateMap<InsertVersionRequest, InsertVersionResponse>();

            this.CreateMap<InsertVersionRequest, VersionRecord>()
                .ForMember(
                    dest => dest.VersionName,
                    opts => opts.MapFrom(src => src.RepositoryVersion));

            this.CreateMap<InsertScriptRunRequest, InsertScriptRunResponse>();

            this.CreateMap<InsertScriptRunRequest, ScriptRunRecord>();

            this.CreateMap<InsertScriptRunErrorRequest, InsertScriptRunErrorResponse>();

            this.CreateMap<InsertScriptRunErrorRequest, ScriptRunErrorRecord>();

            this.CreateMap<MigrateRequest, GetCurrentHashRequest>();

            this.CreateMap<MigrateRequest, GetVersionRequest>();

            this.CreateMap<MigrateRequest, HasScriptRunRequest>();

            this.CreateMap<MigrateRequest, InsertVersionRequest>();

            this.CreateMap<MigrateRequest, InsertScriptRunRequest>();

            this.CreateMap<MigrateRequest, InsertScriptRunErrorRequest>();

            this.CreateMap<ReadFileRequest, ReadFileResponse>();

            this.CreateMap<MigrateRequest, RunScriptRequest>();

            this.CreateMap<ResolveVersionRequest, ResolveVersionResponse>();

            this.CreateMap<RestoreDatabaseRequest, RestoreDatabaseResponse>();

            this.CreateMap<SetupDatabaseRequest, SetupDatabaseResponse>();
        }
    }
}
