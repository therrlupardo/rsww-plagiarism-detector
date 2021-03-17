using System;
using Microsoft.AspNetCore.Mvc;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController
    {
        [HttpGet("{id}")]
        public IActionResult GetAnalysisResult(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}