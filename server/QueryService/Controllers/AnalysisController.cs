using System;
using System.Linq;
using Common.Utils;
using Microsoft.AspNetCore.Mvc;
using QueryService.Dto;
using QueryService.Services;

namespace QueryService.Controllers
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
        ///     Returns data about specified analysis
        /// </summary>
        /// <param name="id">Identifier of analysis</param>
        /// <param name="authorization">JWT token containing user id</param>
        /// <returns>Data about specified analysis</returns>
        /// <response code="200">Data about analysis</response>
        /// <response code="404">Analysis doesn't exist or user didn't perform analysis</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult GetAnalysisResult(Guid id, [FromHeader] string authorization)
        {
            var userId = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var analysis = _analysisService.GetById(id, userId);
                return new OkObjectResult(new AnalysisFileResponse(analysis));
            }
            catch (InvalidOperationException exception)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        ///     Returns list of all analysis performed by current user
        /// </summary>
        /// <param name="authorization">JWT token which contains identifier of current user</param>
        /// <returns>List of analysis performed by current user</returns>
        /// <response code="200">List of analysis (may be empty)</response>
        [HttpGet("all")]
        [Produces("application/json")]
        public IActionResult GetAllAnalysisResults([FromHeader] string authorization)
        {
            var userId = JwtUtil.GetUserIdFromToken(authorization);
            var allAnalysis = _analysisService.GetAllAnalysis(userId);
            var dtos = allAnalysis.Select(analysis => new AnalysisFileResponse(analysis));
            return new OkObjectResult(dtos);
        }
    }
}