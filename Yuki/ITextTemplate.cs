namespace Yuki
{
    using System;
    using Optional;

    public interface ITextTemplate
    {
        Option<string, Exception> Format();
    }
}
