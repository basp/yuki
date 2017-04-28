namespace Yuki.Api.Tags
{
    using System;
    using System.Web.Http;

    [RoutePrefix("api/v1/tags")]
    public class TagsController : ApiController
    {
        [HttpPost]
        [Route]
        public IHttpActionResult CreateTag()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{tagId}")]
        public IHttpActionResult DeleteTag(
            [FromUri] int tagId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{tagId}")]
        public IHttpActionResult UpdateTag(
            [FromUri] int tagId,
            [FromBody] dynamic request)
        {
            throw new NotImplementedException();
        }
    }
}