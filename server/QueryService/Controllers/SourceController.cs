using System;
using Microsoft.AspNetCore.Mvc;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("api/sources")]
    public class SourceController
    {
        [HttpGet("all")]
        [Produces("application/json")]
        public IActionResult GetSources()
        {
            throw new NotImplementedException();
        }
    }
}