namespace Yuki.Commands
{
    using System;
    using System.IO;
    using NLog;
    using Optional;

    using static Optional.Option;

    using Req = SetupDatabaseRequest;
    using Res = SetupDatabaseResponse;

    public class SetupDatabaseCommand : ICommand<Req, Res, Exception>
    {
        private readonly ILogger log = LogManager.GetCurrentClassLogger();

        public SetupDatabaseCommand()
        {
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                var name = Path.GetFileName(request.Folder);
                this.log.Info($"Creating database [{name}] if it does not exist");

                

                return Some<Res, Exception>(new Res());
            }
            catch (Exception ex)
            {
                return None<Res, Exception>(ex);
            }
        }
    }
}
