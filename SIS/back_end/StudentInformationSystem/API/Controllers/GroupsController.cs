using Core.Application.Groups.Commands;
using Core.Application.Groups.Models;
using Core.Application.Groups.Queries;
using Core.Application.QueryUtil;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Core.Domain.Entities;
using IdentityService;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<GroupsController> logger;

        public GroupsController(IMediator mediator, ILogger<GroupsController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(Policy = UserClaims.GroupReadOwn)]
        [HttpGet]
        public async Task<ActionResult<PagedListWrapper<GroupModel>>> GetAll(int page, int pageSize, string filter, string orderBy)
        {
            try
            {
                var response = await mediator.Send(new GetAllGroups.Query()
                {
                    Page = page,
                    PageSize = pageSize,
                    Filter = filter,
                    OrderBy = orderBy
                }).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GroupReadOwn)]
        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDetailsModel>> Get(Guid id)
        {
            try
            {
                var group = await mediator.Send(new GetGroupDetails.Query(id));

                if (group == null)
                {
                    logger.LogDebug("Group not found: " + id);
                    return NotFound();
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GroupCreate)]
        [HttpPost]
        public async Task<ActionResult<GroupModel>> AddGroup([FromBody] AddGroup.Command command)
        {
            try
            {
                if (IsInvalid(command.Name, command.Period, command.StartDate, command.EndDate))
                {
                    return BadRequest("Check if all properties are set and dates are valid.");
                }

                GroupModel response = await mediator.Send(command);

                if (response == null)
                {
                    logger.LogDebug("Could not add group");
                    return NotFound();
                }

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        private static bool IsInvalid(string name, string period, DateTime start, DateTime end)
        {
            return string.IsNullOrEmpty(name) ||
                                string.IsNullOrEmpty(period) ||
                                start == default ||
                                end == default ||
                                !(DateTime.Compare(end, start) == 1);
        }

        [Authorize(Policy = UserClaims.GroupDelete)]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteGroup(Guid id)
        {
            try
            {
                // TODO: not found handling //

                await mediator.Send(new DeleteGroups.Command(id));

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GroupDelete)]
        [HttpPost("deleteMany")]
        public async Task<ActionResult> DeleteGroups(DeleteGroups.Command command)
        {
            try
            {
                // TODO: not found handling //

                await mediator.Send(command);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GroupWrite)]
        [HttpPut("{id}")]
        public async Task<ActionResult<GroupModel>> UpdateGroup(Guid id, [FromBody] UpdateGroup.Command command)
        {
            try
            {
                if (id != command.Id) return BadRequest("Id does not match details.");

                if (IsInvalid(command.Name, command.Period, command.StartDate, command.EndDate))
                {
                    return BadRequest("Check if all properties are set and dates are valid.");
                }

                var response = await mediator.Send(command);

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

        [Authorize(Policy = UserClaims.GroupWrite)]
        [HttpPatch("AddStudentsToGroups")]
        public async Task<ActionResult> AddStudentsToGroups([FromBody] AddStudentsToGroups.Command command)
        {
            // TODO: foutafhandeling!!

            try
            {
                var response = await mediator.Send(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.GroupWrite)]
        [HttpPatch("RemoveStudentsFromGroups")]
        public async Task<ActionResult> RemoveStudentsFromGroups([FromBody] RemoveStudentsFromGroups.Command command)
        {
            // TODO: foutafhandeling!!

            try
            {
                var response = await mediator.Send(command);

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
