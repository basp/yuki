namespace Yuki.Tests
{
    using System.Data;
    using Moq;
    using Xunit;

    public class SqlSessionShould
    {
        [Fact]
        public void StartWithClosedConnection()
        {
            var conn = new Mock<IDbConnection>();
            var session = new SqlSession(conn.Object);

            conn.Verify(x => x.Open(), Times.Never());
        }

        [Fact]
        public void NotOpenMultipleTimes()
        {
            var conn = new Mock<IDbConnection>();
            var session = new SqlSession(conn.Object);
            conn.SetupGet(x => x.State)
                .Returns(ConnectionState.Open);

            session.Open();

            conn.Verify(x => x.Open(), Times.Never());
        }

        [Fact]
        public void BeginTransaction()
        {
            var conn = new Mock<IDbConnection>();
            var session = new SqlSession(conn.Object);

            session.Open();
            session.BeginTransaction();

            conn.Verify(x => x.BeginTransaction(), Times.Once());
        }

        [Fact]
        public void NotStartMultipleTransactions()
        {
            var tx = new Mock<IDbTransaction>();
            var conn = new Mock<IDbConnection>();
            conn.Setup(x => x.BeginTransaction())
                .Returns(tx.Object);

            var session = new SqlSession(conn.Object);

            session.Open();
            session.BeginTransaction();
            session.BeginTransaction();

            conn.Verify(x => x.BeginTransaction(), Times.Once());
        }
    }
}
