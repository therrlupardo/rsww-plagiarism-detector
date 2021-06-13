using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Utils;
using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using OperationContracts;
using OperationContracts.Enums;
using QueryService.Dto;
using QueryService.Dto.Utilities;
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
        [HttpGet("{id:guid}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAnalysisResult(Guid id, [FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var analysis = await _analysisService.GetByIdAsync(model.UserId, id);
                return new OkObjectResult(new AnalysisFileResponse(analysis.ToAnalysisFile(model.UserId)));
            }
            catch (InvalidOperationException exception)
            {
                return new NotFoundResult();
            }
            catch (StreamNotFoundException exception)
            {
                return new NotFoundResult();
            }
        }

        [HttpGet("{id:guid}/report")]
        [Produces("application/pdf")]
        public async Task<IActionResult> GetAnalysisReport([FromRoute] Guid id, [FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var analysis = await _analysisService.GetByIdAsync(id, model.UserId);
                if (analysis.Status != OperationStatus.Complete)
                    return BadRequest();

                var report = _reportService.GenerateReport(analysis.ToAnalysisFile(model.UserId));
                return File(report, "application/pdf");
            }
            catch (InvalidOperationException e)
            {
                return new NotFoundResult();
            }
            catch (StreamNotFoundException exception)
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        ///     Returns list of all analyses performed by current user
        /// </summary>
        /// <param name="authorization">JWT token which contains identifier of current user</param>
        /// <returns>List of analyses performed by current user</returns>
        /// <response code="200">List of analysis (may be empty)</response>
        [HttpGet("all")]
        [Produces("application/json")]
        public async Task<IActionResult> GetUserAnalyses([FromHeader] string authorization)
        {
            var model = JwtUtil.GetUserIdFromToken(authorization);
            try
            {
                var allAnalysis = await _analysisService.GetUserAnalysesAsync(model.UserId);
                var dtos = allAnalysis.Select(analysis => new AnalysisDto(analysis.DocumentName, analysis.DocumentId,
                    analysis.TaskId, Enum.GetName(typeof(OperationStatus), analysis.Status),
                    analysis.LatestChangeDate, analysis.Result));

                return new OkObjectResult(dtos);
            }
            catch (StreamNotFoundException ex)
            {
                return new OkObjectResult(new List<AnalysisDto>());
            }
        }
    }

    public record AnalysisDto(string DocumentName, Guid DocumentId, Guid TaskId, string OperationStatus,
        DateTime LastChangeDate, double Result);
}