using System.Threading.Tasks;
using CommandService.Dto;
using CommandService.Services;
using Common.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/sources")]
    public class SourceController
    {
        private readonly ISourceService _sourceService;

        public SourceController(ISourceService sourceService)
        {
            _sourceService = sourceService;
        }

        /// <summary>
        ///     Upload file which should be set as source file of system
        /// </summary>
        /// <param name="file">File to be set as source file</param>
        /// <param name="authorization">
        ///     JWT token, if it's not correct request won't get through API Gateway, so shouldn' be here
        ///     either
        /// </param>
        /// <returns></returns>
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateSource(IFormFile file, [FromHeader] string authorization)
        {
            var userId = JwtUtil.GetUserIdFromToken(authorization);
            var taskId = await _sourceService.CreateSource(file, userId);
            return new AcceptedResult($"api/source/{taskId}", new CreateSourceResponse(taskId));
        }
    }
}