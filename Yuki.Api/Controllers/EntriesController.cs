namespace Yuki.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Yuki.Model;

    [RoutePrefix("api/entries")]
    public class EntriesController : ApiController
    {
        private readonly Repository repository;

        public EntriesController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        [Route("{entryId}", Name = WellKnownRoutes.GetEntry)]
        public IHttpActionResult Get(int entryId)
        {
            var entry = this.repository.GetEntry(entryId);
            if(entry == null)
            {
                return this.NotFound();
            }

            // Because we have circular reference in our model
            // we _have_ to create a view model here. Otherwise 
            // the JSON serializer will be confuzzled.
            var model = new
            {
                entry.Id,
                entry.Description,
                entry.Duration,
                User = new
                {
                    Id = entry.User.Id,
                    Name = entry.User.Name,
                    Email = entry.User.Email,
                }
            };

            return this.Json(model);
        }
    }
}