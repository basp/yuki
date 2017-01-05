namespace Yuki.Tests
{
    using System;
    using Commands;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using static Optional.Option;

    [TestClass]
    public class ResolveVersionCommandShould
    {
        [TestMethod]
        public void ReturnSomeVersionWhenAbleToResolve()
        {
            var version = Some<string, Exception>("frotz");
            var cmd = new ResolveVersionCommand(x => new FakeVersionResolver(version));
            var req = new ResolveVersionRequest()
            {
                VersionFile = "bar.txt",
            };

            var res = cmd.Execute(req);

            res.MatchSome(x =>
            {
                Assert.AreEqual("frotz", x.VersionName);
                Assert.AreEqual("bar.txt", x.VersionFile);
            });

            res.MatchNone(x => Assert.Fail());
        }

        [TestMethod]
        public void ReturnNoneWhenUnableToResolve()
        {
            var err = new Exception();
            var version = None<string, Exception>(err);
            var cmd = new ResolveVersionCommand(x => new FakeVersionResolver(version));
            var req = new ResolveVersionRequest()
            {
                VersionFile = "bar.txt",
            };

            var res = cmd.Execute(req);

            res.MatchSome(x => Assert.Fail());
            res.MatchNone(x => Assert.IsInstanceOfType(x, typeof(Exception)));
        }
    }
}
