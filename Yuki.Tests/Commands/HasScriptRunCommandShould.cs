namespace Yuki.Tests
{
    using System;
    using Commands;
    using Moq;
    using Optional;
    using Ploeh.AutoFixture;
    using Xunit;
 
    public class HasScriptRunCommandShould
    {
        [Fact]
        public void Execute()
        {
            var fix = new Fixture();

            var hasRun = fix.Create<bool>();

            var repository = new Mock<IRepository>();
            repository
                .Setup(x => x.HasScriptRun(It.IsAny<string>()))
                .Returns(Option.Some<bool, Exception>(hasRun));

            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory
                .Setup(x => x.Create(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(repository.Object);

            var req = fix.Create<HasScriptRunRequest>();

            var cmd = new HasScriptRunCommand(repositoryFactory.Object);

            var res = cmd.Execute(req);

            Assert.True(res.HasValue);

            res.MatchSome(x =>
            {
                Assert.Equal(hasRun, x.HasRunAlready);
                Assert.Equal(req.ScriptName, x.ScriptName);
            });

            repositoryFactory.Verify(x => x.Create(
                req.RepositoryDatabase, 
                req.RepositorySchema));

            repository.Verify(x => x.HasScriptRun(req.ScriptName));
        }
    }
}
