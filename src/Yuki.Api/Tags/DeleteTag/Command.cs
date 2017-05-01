namespace Yuki.Api.Tags.DeleteTag
{
    using System;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                return None<Response, Exception>(new NotImplementedException());
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}