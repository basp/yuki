namespace Yuki.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using Optional;

    using Req = InsertScriptRunErrorRequest;
    using Res = InsertScriptRunErrorResponse;

    public class InsertScriptRunErrorCommand : ICommand<Req, Res, Exception>
    {
        private readonly ISession session;
        private readonly IIdentityProvider identity;

        public InsertScriptRunErrorCommand(ISession session, IIdentityProvider identity)
        {
            Contract.Requires(session != null);
            Contract.Requires(identity != null);

            this.session = session;
            this.identity = identity;
        }

        public Option<Res, Exception> Execute(Req request)
        {
            throw new NotImplementedException();
        }
    }
}
