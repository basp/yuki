namespace Yuki.Tests
{
    using System;
    using Optional;

    public class FakeVersionResolver : IVersionResolver
    {
        private Option<string, Exception> result;

        public FakeVersionResolver(Option<string, Exception> result)
        {
            this.result = result;
        }

        public Option<string, Exception> Resolve()
        {
            return this.result;
        }
    }
}
