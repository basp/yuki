namespace Yuki.Tests
{
    using System;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;

    public class GetCurrentHashCommandShould
    {
        [Fact]
        public void Execute()
        {
            // Arrange
            var fix = new Fixture();

            var hash = fix.Create("hash");

            var repository = new Mock<IRepository>();
            repository
                .Setup(x => x.GetCurrentHash(It.IsAny<string>()))
                .Returns(Option.Some<string, Exception>(hash));

            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(repository.Object);

            var cmd = new GetCurrentHashCommand(repositoryFactory.Object);

            var req = fix.Create<GetCurrentHashRequest>();

            // Act
            var res = cmd.Execute(req);

            // Assert
            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(hash, x.Hash);
                Assert.Equal(req.ScriptName, x.ScriptName);
            });

            repositoryFactory.Verify(
                x => x.Create(req.RepositoryDatabase, req.RepositorySchema),
                Times.Once());

            repository.Verify(
                x => x.GetCurrentHash(req.ScriptName),
                Times.Once());
        }
    }
}
