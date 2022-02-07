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
    public class RolesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<RolesController> logger;

        public RolesController(IMediator mediator, ILogger<RolesController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }




        [Authorize(Policy = UserClaims.AdminRolesRead)]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] GetRoles.Query request, CancellationToken cancellationToken)
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
        public async Task<ActionResult> Get([FromRoute] GetRole.Query request, CancellationToken cancellationToken)
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

        [Authorize(Policy = UserClaims.AdminRolesCreate)]
        [HttpPost]
        public async Task<ActionResult<AppRoleModel>> AddRole(AddRole.Command request, CancellationToken cancellationToken)
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

        [Authorize(Policy = UserClaims.AdminRolesUpdate)]
        [HttpPut("{id}")]
        public async Task<ActionResult<AppUserModel>> UpdateRole([FromBody] UpdateRole.Command command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                if (response == null)
                {
                    logger.LogDebug("Could not find role");
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

        [Authorize(Policy = UserClaims.AdminRolesDelete)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole([FromRoute]string id)
        {
            try
            {

                await mediator.Send(new DeleteRoles.Command { Ids = { id.ToString() } });


                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.AdminRolesDelete)]
        [HttpPost("deleteMany")]
        public async Task<ActionResult> DeleteRoles([FromBody]DeleteRoles.Command request, CancellationToken cancellationToken)
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
    }
}
