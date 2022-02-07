using Core.Domain.Entities;
using IdentityService;
using IdentityService.Commands;
using IdentityService.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<UsersController> logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [Authorize(Policy = UserClaims.AdminUsersRead)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] GetUsers.Query request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.AdminUsersRead)]
        [HttpGet("{Id}")]
        public async Task<ActionResult> Get([FromRoute] GetUser.Query request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.AdminUsersCreate)]
        [HttpPost]
        public async Task<ActionResult<AppUserModel>> AddUser(AddUser.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var grade = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return Ok(grade);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GradeDelete)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            try
            {

                await mediator.Send(new DeleteUsers.Command { Ids = { id.ToString() } });

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.AdminUserDelete)]
        [HttpPost("deleteMany")]
        public async Task<ActionResult> DeleteUser([FromBody] DeleteUsers.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.AdminUserUpdate)]
        [HttpPut("{Id}")]
        public async Task<ActionResult<AppUserModel>> UpdateUser([FromBody] UpdateUser.Command command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                if (response == null)
                {
                    logger.LogDebug("Could not find user");
                    return NotFound();
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }
    }
}
