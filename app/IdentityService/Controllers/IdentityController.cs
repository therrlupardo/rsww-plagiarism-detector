using System;
using IdentityService.Dto;
using IdentityService.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/identity")]
    [EnableCors("AllowAllHeaders")]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        [Route("exception")]
        public IActionResult ThrowException()
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        ///     Checks if given credentials are valid and returns token
        /// </summary>
        /// <response code="200">Returns object with accessToken</response>
        /// <response code="401">Returned if credentials are not valid</response>
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var success = _identityService.TryToLogin(loginRequest, out var token);
            return success ? new OkObjectResult(new LoginResponse(token)) : new UnauthorizedResult();
        }

        /// <summary>
        ///     Returns list of all accounts
        /// </summary>
        /// <returns>List of accounts</returns>
        /// <response code="200">Return list of users</response>
        [HttpGet]
        [Route("all")]
        public IActionResult GetAccounts()
        {
            return new OkObjectResult(_identityService.GetAccounts());
        }

        /// <summary>
        ///     Creates new user
        /// </summary>
        /// <returns>Data of created user</returns>
        /// <response code="201">User created successfully. Response contains data of user</response>
        /// <response code="400">Error while adding user, more info about it in body</response>
        [HttpPost]
        [Route("create")]
        public IActionResult CreateUser([FromBody] CreateAccountRequest request)
        {
            try
            {
                var user = _identityService.CreateUser(request.Login, request.Password);
                return new CreatedResult("", user);
            }
            catch (ArgumentException e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}