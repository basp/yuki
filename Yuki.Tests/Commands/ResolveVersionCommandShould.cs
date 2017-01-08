namespace Yuki.Tests
{
    using System;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;

    public class ResolveVersionCommandShould
    {
        [Fact]
        public void Execute()
        {
            // Arrange
            var fix = new Fixture();

            var versionName = fix.Create("versionName");

            var resolver = new Mock<IVersionResolver>();
            resolver
                .Setup(x => x.Resolve())
                .Returns(Option.Some<string, Exception>(versionName));

            var resolverFactory = new Mock<IVersionResolverFactory>();
            resolverFactory
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(resolver.Object);

            var req = fix.Create<ResolveVersionRequest>();

            var cmd = new ResolveVersionCommand(resolverFactory.Object.Create);

            // Act
            var res = cmd.Execute(req);

            // Assert
            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(req.VersionFile, x.VersionFile);
                Assert.Equal(versionName, x.VersionName);
            });

            resolverFactory.Verify(x => x.Create(req.VersionFile));
        }
    }
}
