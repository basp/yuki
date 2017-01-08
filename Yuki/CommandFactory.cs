namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using Commands;

    public class CommandFactory : ICommandFactory
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IHasher hasher;
        private readonly ITextTemplateFactory textTemplateFactory;
        private readonly Func<ISession, IRepositoryFactory> repositoryFactoryFactory;
        private readonly Func<ISession, IDatabaseFactory> databaseFactoryFactory;
        private readonly Func<string, IVersionResolver> resolverFactory;

        public CommandFactory(
            IIdentityProvider identityProvider,
            IHasher hasher,
            ITextTemplateFactory textTemplateFactory,
            Func<ISession, IRepositoryFactory> repositoryFactoryFactory,
            Func<ISession, IDatabaseFactory> databaseFactoryFactory,
            Func<string, IVersionResolver> resolverFactory)
        {
            Contract.Requires(identityProvider != null);
            Contract.Requires(hasher != null);
            Contract.Requires(textTemplateFactory != null);
            Contract.Requires(repositoryFactoryFactory != null);
            Contract.Requires(databaseFactoryFactory != null);
            Contract.Requires(resolverFactory != null);

            this.identityProvider = identityProvider;
            this.hasher = hasher;
            this.textTemplateFactory = textTemplateFactory;
            this.repositoryFactoryFactory = repositoryFactoryFactory;
            this.databaseFactoryFactory = databaseFactoryFactory;
            this.resolverFactory = resolverFactory;
        }

        public IGetCurrentHashCommand CreateGetCurrentHashCommand(ISession session) =>
            new GetCurrentHashCommand(this.repositoryFactoryFactory(session));

        public IGetVersionCommand CreateGetVersionCommand(ISession session) =>
            new GetVersionCommand(this.repositoryFactoryFactory(session));

        public IInitializeRepositoryCommand CreateInitializeRepositoryCommand(ISession session) =>
            new InitializeRepositoryCommand(this.repositoryFactoryFactory(session));

        public IInsertScriptRunCommand CreateInsertScriptRunCommand(ISession session) =>
            new InsertScriptRunCommand(session, this.identityProvider);

        public IInsertScriptRunErrorCommand CreateInsertScriptRunErrorCommand(ISession session) =>
            new InsertScriptRunErrorCommand(session, this.identityProvider);

        public IInsertVersionCommand CreateInsertVersionCommand(ISession session) =>
            new InsertVersionCommand(session, this.identityProvider);

        public IReadFileCommand CreateReadFileCommand() =>
            new ReadFileCommand(this.hasher);

        public IResolveVersionCommand CreateResolveVersionCommand() =>
            new ResolveVersionCommand(this.resolverFactory);

        public IRestoreDatabaseCommand CreateRestoreDatabaseCommand(ISession session) =>
            new RestoreDatabaseCommand(this.databaseFactoryFactory(session));

        public ICreateDatabaseCommand CreateCreateDatabaseCommand(ISession session) =>
            new CreateDatabaseCommand(this.databaseFactoryFactory(session));

        public ISetupDatabaseCommand CreateSetupDatabaseCommand(ISession session) =>
            new SetupDatabaseCommand(
                this.CreateCreateDatabaseCommand(session),
                this.CreateRestoreDatabaseCommand(session));

        public IHasScriptRunCommand CreateHasScriptRunCommand(ISession session) =>
            new HasScriptRunCommand(this.repositoryFactoryFactory(session));
    }
}
