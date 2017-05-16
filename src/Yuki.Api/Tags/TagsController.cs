namespace Yuki.Api.Tags
{
    using System;
    using System.Web.Http;
    using Yuki.Data;

    [RoutePrefix("api/tags")]
    public class TagsController : ApiController
    {
        private readonly Repository<Tag> repository;

        public TagsController(Repository<Tag> repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route(Name = nameof(CreateTag))]
        public IHttpActionResult CreateTag(
            [FromBody] CreateTag.Request request)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{tagId}", Name = nameof(DeleteTag))]
        public IHttpActionResult DeleteTag(
            [FromUri] int tagId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{tagId}", Name = nameof(UpdateTag))]
        public IHttpActionResult UpdateTag(
            [FromUri] int tagId,
            [FromBody] UpdateTag.Request request)
        {
            throw new NotImplementedException();
        }
    }
}