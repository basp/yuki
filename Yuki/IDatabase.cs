namespace Yuki
{
    using System;
    using Optional;

    public interface IDatabase
    {
        Option<bool, Exception> Create();

        Option<int, Exception> Drop();

        Option<bool, Exception> Restore(string backup);
    }
}
