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

        /// <summary>
        /// Checks if given credentials are valid and returns token
        /// </summary>
        /// <param name="login">Login of user</param>
        /// <param name="password">Password of user</param>
        /// <response code="200">Returns object with accessToken</response>
        /// <response code="401">Returned if credentials are not valid</response>
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromQuery] string login, [FromQuery] string password)
        {
            try
            {
                var token = _identityService.Login(login, password);
                return new OkObjectResult(new LoginResponse(token));
            }
            catch (NullReferenceException e)
            {
                return new UnauthorizedResult();
            }

            ;
        }

        /// <summary>
        /// Returns list of all accounts
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
        /// Creates new user
        /// </summary>
        /// <param name="login">Login of new user</param>
        /// <param name="password">Password of new user</param>
        /// <returns>Data of created user</returns>
        /// <response code="201">User created successfully. Response contains data of user</response>
        /// <response code="400">Error while adding user, more info about it in body</response>
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