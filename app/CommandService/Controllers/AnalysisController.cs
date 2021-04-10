using System;
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
    public class AnalysisController : Controller
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
        /// <response code="200">File send to be analysed (analysis not started yet) </response>
        /// <returns></returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendFileToAnalysis(IFormFile file, [FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            var fileId = await _analysisService.SendFileToAnalysis(file, model.UserId);
            return new OkObjectResult(new SendFileToAnalysisResponse(fileId));
        }

        /// <summary>
        ///     Sends command to start analysis of specified file
        /// </summary>
        /// <param name="id">Identifier of file to be analysed</param>
        /// <param name="authorization">JWT token, if not set request should be stopped at API Gateway</param>
        /// <response code="202">Analysis started</response>
        /// <returns></returns>
        [HttpPost("{id}/start")]
        public async Task<IActionResult> AnalyzeFile([FromRoute] Guid id, [FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            var taskId = await _analysisService.AnalyzeFile(id, model.UserId);
            return new AcceptedResult($"api/analysis/{taskId}", new AnalysisResponse(taskId));
        }
    }
}