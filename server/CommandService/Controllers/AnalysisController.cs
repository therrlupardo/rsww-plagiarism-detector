using System.Threading.Tasks;
using CommandService.Dto;
using CommandService.Services;
using Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }

        /// <summary>
        ///     Sends file to be analysed
        /// </summary>
        /// <param name="file">File to be analysed</param>
        /// <param name="authorization">JWT token, if not set request should be stopped at API Gateway</param>
        /// <response code="202">File send to be analysed. </response>
        /// <returns></returns>
        [HttpPost("perform")]
        public async Task<IActionResult> PerformAnalysis(IFormFile file, [FromHeader] string authorization)
        {
            var userId = JwtUtil.GetUserIdFromToken(authorization);
            var taskId = await _analysisService.PerformAnalysis(file, userId);
            return new AcceptedResult($"api/analysis/{taskId}", new PerformAnalysisResponse(taskId));
        }
    }
}