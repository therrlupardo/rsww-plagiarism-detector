using System;
using Microsoft.AspNetCore.Mvc;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("/api/reports")]
    public class ReportController
    {
        [HttpGet("{id}")]
        public IActionResult GetReport(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}