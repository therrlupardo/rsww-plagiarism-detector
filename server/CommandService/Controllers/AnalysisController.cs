using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController
    {
        [HttpPost("perform")]
        public IActionResult AnalyzeFile()
        {
            throw new NetworkInformationException();
        }
    }
}