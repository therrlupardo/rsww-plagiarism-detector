using System;
using IdentityService.Dto;
using IdentityService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/identity")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Get([FromQuery] string name, [FromQuery] string password)
        {
            var token = _identityService.Login(name, password);
            if (token == null)
                return new UnauthorizedResult();
            return new OkObjectResult(new LoginResponse(token));
        }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            return new OkObjectResult(_identityService.GetAll());
        }

        [HttpPost]
        [Route("create")]
        public IActionResult CreateUser([FromQuery] string login, [FromQuery] string password)
        {
            try
            {
                var user = _identityService.CreateUser(login, password);
                return new CreatedResult("", user);
            }
            catch (ArgumentException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}