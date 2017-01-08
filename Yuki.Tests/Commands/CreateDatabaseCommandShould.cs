namespace Yuki.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;

    public class CreateDatabaseCommandShould
    {
        [Fact]
        public void Execute()
        {
            // Arrange
            var fix = new Fixture();

            var created = fix.Create<bool>();

            var database = new Mock<IDatabase>();
            database.Setup(x => x.Create())
                .Returns(Option.Some<bool, Exception>(true));

            var databaseFactory = new Mock<IDatabaseFactory>();
            databaseFactory
                .Setup(x => x.Create(It.IsAny<string>()))
                .Returns(database.Object);

            var req = fix.Create<CreateDatabaseRequest>();

            var cmd = new CreateDatabaseCommand(databaseFactory.Object);

            // Act
            var res = cmd.Execute(req);

            // Assert
            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(req.Server, x.Server);
                Assert.Equal(req.Database, x.Database);
                Assert.Equal(true, x.Created);
            });
        }
    }
}
