namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using AutoMapper;
    using Optional;

    using Req = CreateDatabaseRequest;
    using Res = CreateDatabaseResponse;

    public class CreateDatabaseCommand : ICreateDatabaseCommand
    {
        private readonly IDatabaseFactory databaseFactory;

        public CreateDatabaseCommand(
            IDatabaseFactory databaseFactory)
        {
            Contract.Requires(databaseFactory != null);
            this.databaseFactory = databaseFactory;
        }

        public Option<Res, Exception> Execute(Req req)
        {
            var database = this.databaseFactory.Create(req.Database);
            return database.Create()
                .Map(x => CreateResponse(req, x));
        }

        private static Res CreateResponse(Req req, bool created)
        {
            var res = new Res { Created = created };
            return Mapper.Map(req, res);
        }
    }
}
