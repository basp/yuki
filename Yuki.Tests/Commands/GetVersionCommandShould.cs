namespace Yuki.Tests
{
    using System;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;

    public class GetVersionCommandShould
    {
        [Fact]
        public void Execute()
        {
            // Arrange
            var fix = new Fixture();

            var versionName = fix.Create("versionName");

            var repository = new Mock<IRepository>();
            repository
                .Setup(x => x.GetVersion(It.IsAny<string>()))
                .Returns(Option.Some<string, Exception>(versionName));

            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(repository.Object);

            var req = fix.Create<GetVersionRequest>();

            var cmd = new GetVersionCommand(repositoryFactory.Object);

            // Act
            var res = cmd.Execute(req);

            // Assert
            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(versionName, x.VersionName);
            });

            repositoryFactory.Verify(
                x => x.Create(
                    req.RepositoryDatabase,
                    req.RepositorySchema),
                Times.Once());

            repository.Verify(
                x => x.GetVersion(req.RepositoryPath),
                Times.Once());
        }
    }
}
