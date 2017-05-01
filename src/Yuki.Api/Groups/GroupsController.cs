namespace Yuki.Api.Groups
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/groups")]
    public class GroupsController : ApiController
    {
        private readonly Repository<Group> repository;

        public GroupsController(Repository<Group> repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route(Name = nameof(CreateGroup))]
        public IHttpActionResult CreateGroup(
            [FromBody] CreateGroup.Request request)
        {
            var cmd = new CreateGroup.Command(this.repository);
            var res = cmd.Execute(request);
            return res.Match<IHttpActionResult>(
                some => this.Json(some),
                none => this.InternalServerError(none));
        }

        [HttpDelete]
        [Route("{groupId}", Name = nameof(DeleteGroup))]
        public IHttpActionResult DeleteGroup(
            [FromUri] int groupId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{groupId}", Name = nameof(UpdateGroup))]
        public IHttpActionResult UpdateGroup(
            [FromUri] int groupId,
            [FromBody] UpdateGroup.Request request)
        {
            request.GroupId = groupId;
            var cmd = new UpdateGroup.Command(this.repository);
            var res = cmd.Execute(request);
            return res.Match<IHttpActionResult>(
                some => this.Json(some),
                none => this.InternalServerError(none));
        }
    }
}