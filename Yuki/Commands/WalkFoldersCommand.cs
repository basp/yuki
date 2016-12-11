namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;

    using Optional;

    using static Optional.Option;

    using Req = WalkFoldersRequest;
    using Res = WalkFoldersResponse;

    public class WalkFoldersCommand : ICommand<Req, Res, Exception>
    {
        private readonly Func<string, Option<Res, Exception>> walker;

        public WalkFoldersCommand(
            Func<string, Option<Res, Exception>> walker)
        {
            Contract.Requires(walker != null);

            this.walker = walker;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(request.Folder))
                {
                    var res = this.walker(dir);
                    if (!res.HasValue)
                    {
                        return res;
                    }
                }

                return Some<Res, Exception>(Res.Done);
            }
            catch (Exception ex)
            {
                return None<Res, Exception>(ex);
            }
        }
    }
}
