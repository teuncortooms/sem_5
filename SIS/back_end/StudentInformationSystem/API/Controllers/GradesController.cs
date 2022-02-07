using Core.Application.Grades.Commands;
using Core.Application.Grades.Queries;
using Core.Application.Models;
using Core.Application.QueryUtil;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IdentityService;
using Core.Domain.Entities;
using System.Security.Principal;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<GradesController> logger;

        public GradesController(IMediator mediator, ILogger<GradesController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PagedListWrapper<GradeViewModel>>> GetAll([FromQuery]GetGrades.Query request, CancellationToken cancellationToken)
        {
            try
            {
                var grades = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

                if (grades == null)
                {
                    logger.LogDebug("No grades found");
                    return NotFound();
                }

                return Ok(grades);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GradeViewModel>> Get([FromRoute]GetGrade.Query request, CancellationToken cancellationToken)
        {
            return await mediator.Send(request, cancellationToken).ConfigureAwait(false);

        }

        [Authorize(Policy = UserClaims.GradeCreate)]
        [HttpPost]
        public async Task<ActionResult<GradeViewModel>> AddGrade(AddGrade.Command request, CancellationToken cancellationToken )
        {
            try
            {
                var grade = await mediator.Send(request,cancellationToken).ConfigureAwait(false);

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
        public async Task<ActionResult> DeleteGrade(Guid id)
        {
            try
            {
                
                await mediator.Send(new DeleteGrades.Command { Ids = { id } });

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GradeDelete)]
        [HttpPost("deleteMany")]
        public async Task<ActionResult<GradeViewModel>> RemoveGrade(DeleteGrades.Command request, CancellationToken cancellationToken)
        {
            try
            {
                var grade = await mediator.Send(request, cancellationToken).ConfigureAwait(false);

                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GradeWrite)]
        [HttpPut("{id}")]
        public async Task<ActionResult<GradeViewModel>> UpdateGrade([FromBody] UpdateGrade.Command command, CancellationToken cancellationToken)
        {
            try
            {               
                var response = await mediator.Send(command, cancellationToken).ConfigureAwait(false);

                if (response == null)
                {
                    logger.LogDebug("Could not find group");
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
