namespace Yuki.Commands
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;

    using Optional;

    using static Optional.Option;

    using Req = WalkFoldersRequest;

    public class WalkFoldersCommand<TWalkRes> : ICommand<Req, IList<TWalkRes>, Exception>
    {
        private readonly Func<string, Option<TWalkRes, Exception>> walker;

        public WalkFoldersCommand(
            Func<string, Option<TWalkRes, Exception>> walker)
        {
            Contract.Requires(walker != null);

            this.walker = walker;
        }

        public Option<IList<TWalkRes>, Exception> Execute(Req request)
        {
            try
            {
                // We need to help the type inference out a little bit here.
                IList<TWalkRes> results = new List<TWalkRes>();
                foreach (var dir in Directory.GetDirectories(request.Folder))
                {
                    var res = this.walker(dir);
                    if (!res.HasValue)
                    {
                        return res.Map(x => results);
                    }

                    res.MatchSome(x => results.Add(x));
                    res.MatchNone(x =>
                    {
                        // Should never happen.
                        throw new Exception("TILT");
                    });
                }

                return Some<IList<TWalkRes>, Exception>(results);
            }
            catch (Exception ex)
            {
                return None<IList<TWalkRes>, Exception>(ex);
            }
        }
    }
}
