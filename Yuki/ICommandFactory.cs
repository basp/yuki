namespace Yuki
{
    using Commands;

    public interface ICommandFactory
    {
        IReadFileCommand CreateReadFileCommand();

        IResolveVersionCommand CreateResolveVersionCommand();

        ISetupDatabaseCommand CreateSetupDatabaseCommand(
            ISession session);

        IInitializeRepositoryCommand CreateInitializeRepositoryCommand(
            ISession session);

        IGetVersionCommand CreateGetVersionCommand(
            ISession session);

        IInsertVersionCommand CreateInsertVersionCommand(
            ISession session);

        IInsertScriptRunCommand CreateInsertScriptRunCommand(
            ISession session);

        IInsertScriptRunErrorCommand CreateInsertScriptRunErrorCommand(
            ISession session);

        IGetCurrentHashCommand CreateGetCurrentHashCommand(
            ISession session);

        IHasScriptRunCommand CreateHasScriptRunCommand(
            ISession session);
    }
}
