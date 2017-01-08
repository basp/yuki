namespace Yuki.Tests
{
    using System;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;

    public class InitializeRepositoryCommandShould
    {
        [Fact]
        public void Execute()
        {
            // Arrange
            var fix = new Fixture();

            var initialized = fix.Create<bool>();

            var repository = new Mock<IRepository>();
            repository
                .Setup(x => x.Initialize())
                .Returns(Option.Some<bool, Exception>(initialized));

            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(repository.Object);

            var req = fix.Create<InitializeRepositoryRequest>();

            var cmd = new InitializeRepositoryCommand(repositoryFactory.Object);

            // Act
            var res = cmd.Execute(req);

            // Assert
            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(req.RepositoryDatabase, x.RepositoryDatabase);
                Assert.Equal(req.RepositorySchema, x.RepositorySchema);
                Assert.Equal(req.Server, x.Server);
            });

            repositoryFactory.Verify(
                x => x.Create(
                    req.RepositoryDatabase,
                    req.RepositorySchema),
                Times.Once());

            repository.Verify(x => x.Initialize(), Times.Once());
        }
    }
}
