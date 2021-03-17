using System;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/sources")]
    public class SourceController
    {
        [HttpPost("create")]
        public IActionResult CreateSource()
        {
            throw new NotImplementedException();
        }
    }
}