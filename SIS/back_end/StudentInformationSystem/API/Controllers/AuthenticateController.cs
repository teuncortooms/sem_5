using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth;
using IdentityService;
using IdentityService.Commands;
using IdentityService.Commands.ResponseModels;
using IdentityService.Queries.ResponseModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<AdminController> logger;

        public AuthenticateController(IMediator mediator, ILogger<AdminController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register.Command command,
            CancellationToken cancellationToken)
        {
            try
            {
                if (command == null || !ModelState.IsValid) // TODO: FIXME: Service depends on Presentation?!
                    return BadRequest("Invalid input");

                RegisterResponse response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                return response.StatusCode == RegistrationStatus.Error
                    ? BadRequest(response)
                    : Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }


        [HttpPost]
        [Route("register-admin")]
        [Authorize(Policy = PolicyNames.AdminOnly)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdmin.Command command,
            CancellationToken cancellationToken)
        {
            try
            {
                RegisterResponse response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                return response.StatusCode == RegistrationStatus.Error
                    ? StatusCode(StatusCodes.Status500InternalServerError, response)
                    : Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login.Command command, CancellationToken cancellationToken)
        {
            try
            {
                LoginResponse response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                return response.StatusCode == LoginStatus.Unauthorized
                    ? Unauthorized()
                    : Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [HttpPost]
        [Route("ExternalLogin")]
        public async Task<IActionResult> ExternalLogin([FromBody] ExternalLogin.Command command, CancellationToken cancellationToken)
        {
            try
            {
                LoginResponse response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                return response.StatusCode == LoginStatus.Unauthorized
                    ? Unauthorized()
                    : Ok(response);
            }
            catch (InvalidJwtException ex)
            {
                logger.LogError(ex, "External login token validation failed");
                return BadRequest("External login token validation failed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [HttpGet]
        [Route("details")]
        public IActionResult GetUserDetails(CancellationToken cancellationToken)
        {
            // var response = await mediator.Send(new GetUserDetails.Query(), cancellationToken);

            var response = new UserDetailsModel
            {
                Username = User.Claims.FirstOrDefault(c => c.Type.ToString().Contains("name"))?.Value,
                Roles = User.Claims.Where(c => c.Type.ToString().Contains("role")).Select(c => c.Value).ToList(),
                Claims = User.Claims.Select(c => new ClaimModel { Type = c.Type.ToString(), Value = c.Value.ToString() }).ToList()
            };

            return Ok(response);
        }
    }
}
