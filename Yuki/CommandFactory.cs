namespace Yuki
{
    using System;
    using System.Diagnostics.Contracts;
    using Commands;

    public class CommandFactory : ICommandFactory
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IHasher hasher;
        private readonly Func<string, IVersionResolver> resolverFactory;

        public CommandFactory(
            IIdentityProvider identityProvider,
            IHasher hasher,
            Func<string, IVersionResolver> resolverFactory)
        {
            Contract.Requires(identityProvider != null);
            Contract.Requires(hasher != null);
            Contract.Requires(resolverFactory != null);

            this.identityProvider = identityProvider;
            this.hasher = hasher;
            this.resolverFactory = resolverFactory;
        }

        public IGetCurrentHashCommand CreateGetCurrentHashCommand(ISession session) =>
            new GetCurrentHashCommand(session);

        public IGetVersionCommand CreateGetVersionCommand(ISession session) =>
            new GetVersionCommand(session);

        public IInitializeRepositoryCommand CreateInitializeRepositoryCommand(ISession session) =>
            new InitializeRepositoryCommand(session);

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

        public IRunScriptsCommand CreateRunScriptsCommand(ISession session, IMigrator migrator) =>
            new RunScriptsCommand(session, this, migrator);

        public IRestoreDatabaseCommand CreateRestoreDatabaseCommand(ISession session) =>
            new RestoreDatabaseCommand(session);

        public ICreateDatabaseCommand CreateCreateDatabaseCommand(ISession session) =>
            new CreateDatabaseCommand(session);

        public ISetupDatabaseCommand CreateSetupDatabaseCommand(ISession session) =>
            new SetupDatabaseCommand(
                this.CreateCreateDatabaseCommand(session),
                this.CreateRestoreDatabaseCommand(session));

        public IHasScriptRunCommand CreateHasScriptRunCommand(ISession session) =>
            new HasScriptRunCommand(session);
    }
}
