using System;
using System.Linq;
using Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Queries;
using QueryService.Dto;
using QueryService.Services;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("api/analysis")]
    public class AnalysisController : Controller
    {
        private readonly IAnalysisService _analysisService;
        private readonly IReportService<AnalysisFile> _reportService;

        public AnalysisController(IAnalysisService analysisService, IReportService<AnalysisFile> reportService)
        {
            _analysisService = analysisService;
            _reportService = reportService;
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
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var analysis = _analysisService.GetById(id, model.UserId);
                return new OkObjectResult(new AnalysisFileResponse(analysis));
            }
            catch (InvalidOperationException exception)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("{id}/report")]
        [Produces("application/pdf")]
        public IActionResult GetAnalysisReport([FromRoute] Guid id, [FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var analysis = _analysisService.GetById(id, model.UserId);
                var report = _reportService.GenerateReport(analysis);
                return File(report, "application/pdf");
            }
            catch (InvalidOperationException e)
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
            var model = JwtUtil.GetUserIdFromToken(authorization);
            var allAnalysis = _analysisService.GetAllAnalysis(model.UserId);
            var dtos = allAnalysis.Select(analysis => new AnalysisFileResponse(analysis));
            return new OkObjectResult(dtos);
        }
    }
}