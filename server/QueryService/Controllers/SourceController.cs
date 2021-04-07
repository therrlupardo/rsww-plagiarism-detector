using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QueryService.Dto;
using QueryService.Services;

namespace QueryService.Controllers
{
    [ApiController]
    [Route("api/sources")]
    public class SourceController : Controller
    {
        private readonly ISourceService _sourceService;

        public SourceController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        /// <summary>
        ///     Returns basic info about all source files
        /// </summary>
        /// <returns>List of all source files</returns>
        /// <response code="200">List of all source files (may be empty)</response>
        [HttpGet("all")]
        [Produces("application/json")]
        public async Task<IActionResult> GetAllSources()
        {
            var sourceFiles = await _sourceService.GetAllSourceFilesAsync();
            var dtos = sourceFiles.Select(f => new SourceFileResponse(f));
            return new OkObjectResult(dtos);
        }

        /// <summary>
        ///     Returns data about source file with given id
        /// </summary>
        /// <param name="id">Identifier of source file</param>
        /// <returns>Date about source file</returns>
        /// <response code="200">Data about source file</response>
        /// <response code="404">Source file with given id doesn't exist</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult GetSourceStatus([FromRoute] Guid id)
        {
            try
            {
                var sourceFile = _sourceService.GetById(id);
                return new OkObjectResult(new SourceFileResponse(sourceFile));
            }
            catch (InvalidOperationException e)
            {
                return new NotFoundResult();
            }
        }
    }
}