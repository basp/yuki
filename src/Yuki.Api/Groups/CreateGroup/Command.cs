namespace Yuki.Api.Groups.CreateGroup
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Optional;
    using Yuki.Data;

    using static Optional.Option;

    public class Command : ICommand<Request, Response, Exception>
    {
        private readonly Repository<Group> repository;

        public Command(Repository<Group> repository)
        {
            this.repository = repository;
        }

        public Option<Response, Exception> Execute(Request req)
        {
            try
            {
                var group = Mapper.Map<Group>(req.Group);
                this.repository.Insert(group);

                var data = Mapper.Map<IDictionary<string, object>>(group);
                var res = new Response(data);

                return Some<Response, Exception>(res);
            }
            catch (Exception ex)
            {
                return None<Response, Exception>(ex);
            }
        }
    }
}