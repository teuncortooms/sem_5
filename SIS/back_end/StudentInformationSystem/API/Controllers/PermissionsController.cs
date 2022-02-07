using IdentityService.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityService;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<PermissionsController> logger;

        public PermissionsController(IMediator mediator, ILogger<PermissionsController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] GetPermissions.Query request, CancellationToken cancellationToken)
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
