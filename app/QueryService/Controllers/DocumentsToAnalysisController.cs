using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Utils;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using QueryService.Dto;
using QueryService.Services;
using QueryService.Services.Implementations;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("api/documentsToAnalysis")]
    public class DocumentsToAnalysisController : Controller
    {
        private readonly IDocumentsToAnalysisService _documentsToAnalysisService;

        public DocumentsToAnalysisController(IDocumentsToAnalysisService documentsToAnalysisService)
        {
            _documentsToAnalysisService = documentsToAnalysisService;
        }


        /// <summary>
        ///     Returns data about specified analysis
        /// </summary>
        /// <param name="authorization">JWT token containing user id</param>
        /// <returns>Data about specified analysis</returns>
        /// <response code="200">List of files which are ready to undergo analysis</response>
        /// <response code="404"></response>
        [HttpGet("all")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUsersDocumentsToAnalysis([FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var documents = await _documentsToAnalysisService.GetDocumentsToAnalysis(model.UserId);
                return new OkObjectResult(documents);
            }
            catch (StreamNotFoundException e)
            {
                return new OkObjectResult(new List<DocumentToAnalysisResponse>());
            }

        }

        [HttpGet("withLatestStatus")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserDocumentsWithLatestStatuses([FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var documents = await _documentsToAnalysisService.GetDocumentsWithLatestAnalysisStatuses(model.UserId);
                return new OkObjectResult(documents);
            }
            catch (StreamNotFoundException ex)
            {
                return new OkObjectResult(new List<AnalysisStatusDto>());
            }
        }

    }
}