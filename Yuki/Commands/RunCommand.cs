namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using Req = RunRequest;
    using Res = RunResponse;

    public class RunCommand : ICommand<RunRequest, RunResponse, Exception>
    {
        private readonly ISessionFactory sessionFactory;

        public RunCommand(ISessionFactory sessionFactory)
        {
            Contract.Requires(sessionFactory != null);

            this.sessionFactory = sessionFactory;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            throw new NotImplementedException();
        }
    }
}
