namespace Yuki.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class WorkspacesController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var xs = new[] { "foo", "bar", "quux" };
            return this.Json(xs);
        }
    }
}
