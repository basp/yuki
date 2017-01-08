namespace Yuki.Tests
{
    using System;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;

    public class InsertVersionCommandShould
    {
        [Fact]
        public void Execute()
        {
            // Arrange
            var fix = new Fixture();

            var versionId = fix.Create<int>();

            var user = fix.Create<string>();

            var repository = new Mock<IRepository>();
            repository
                .Setup(x => x.InsertVersion(It.IsAny<VersionRecord>()))
                .Returns(Option.Some<int, Exception>(versionId));

            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(repository.Object);

            var identityProvider = new Mock<IIdentityProvider>();
            identityProvider
                .Setup(x => x.GetCurrent())
                .Returns(Option.Some<string, Exception>(user));

            var req = fix.Create<InsertVersionRequest>();

            var cmd = new InsertVersionCommand(
                repositoryFactory.Object,
                identityProvider.Object);

            // Act
            var res = cmd.Execute(req);

            // Assert
            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(versionId, x.VersionId);
            });

            repositoryFactory.Verify(
                x => x.Create(req.RepositoryDatabase, req.RepositorySchema),
                Times.Once());

            repository.Verify(
                x => x.InsertVersion(It.IsAny<VersionRecord>()),
                Times.Once());

            identityProvider.Verify(x => x.GetCurrent(), Times.Once());
        }
    }
}
