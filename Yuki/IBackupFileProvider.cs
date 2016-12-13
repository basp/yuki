namespace Yuki
{
    using System;
    using Optional;

    public interface IBackupFileProvider
    {
        Option<string, Exception> TryFindIn(string folder);
    }
}
