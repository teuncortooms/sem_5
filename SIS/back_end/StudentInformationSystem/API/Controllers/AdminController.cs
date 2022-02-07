using Core.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IdentityService;
using IdentityService.Queries;
using IdentityService.Commands;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<AdminController> logger;

        public AdminController(IMediator mediator, ILogger<AdminController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Route("SeedData")]
        // no authorization, uses secret
        public async Task<ActionResult> SeedData( PerformSeeding.Command command, CancellationToken cancellationToken)
        { 
            try
            {
                await mediator.Send(command, cancellationToken).ConfigureAwait(false);
                await mediator.Send(new SeedUsersAndRoles.Command(), cancellationToken).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }





    }
}
