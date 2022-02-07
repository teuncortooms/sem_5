using Core.Application.QueryUtil;
using Core.Application.Teachers.Commands;
using Core.Application.Teachers.Models;
using Core.Application.Teachers.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using static Core.Application.Teachers.Commands.AddTeacher;
using Core.Domain.Entities;
using FluentValidation;
using IdentityService;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<TeachersController> logger;

        public TeachersController(IMediator mediator, ILogger<TeachersController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Authorize(Policy = UserClaims.TeacherRead)]
        [HttpGet]
        public async Task<ActionResult<PagedListWrapper<TeacherModel>>> GetAll(int page, int pageSize, string filter, string orderBy)
        {
            try
            {
                var response = await mediator.Send(new GetTeachers.Query()
                {
                    Page = page,
                    PageSize = pageSize,
                    Filter = filter,
                    OrderBy = orderBy
                });

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.TeacherRead)]
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherModel>> Get(Guid id)
        {
            try
            {
                TeacherModel teacher = await mediator.Send(new GetTeacherById.Query(id));

                if (teacher == null)
                {
                    logger.LogDebug("Not found: " + id);
                    return NotFound();
                }

                return Ok(teacher);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.TeacherCreate)]
        // POST api/<ValuesController>
        [HttpPost]
        public async Task<ActionResult<TeacherModel>> AddTeacher([FromBody] AddTeacherCommand command)
        {
            try
            {
                if (IsInvalid(command.FirstName, command.LastName))
                {
                    return BadRequest("Check if all properties are set.");
                }

                TeacherModel response = await mediator.Send(command);

                if (response == null)
                {
                    logger.LogDebug("Could not add student");
                    return NotFound();
                }

                return CreatedAtAction(nameof(Get), new { id = response.Id }, response);

            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.TeacherWrite)]
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TeacherModel>> UpdateTeacher(Guid id, [FromBody] UpdateTeacher.Command command)
        {
            try
            {
                if (id != command.Id) return BadRequest("Id does not match details.");

                if (IsInvalid(command.FirstName, command.LastName))
                {
                    return BadRequest("Check if all properties are set.");
                }

                var response = await mediator.Send(command);

                if (response == null)
                {
                    logger.LogDebug("Could not find student");
                    return NotFound();
                }

                return Ok(response);
            }
            catch(ValidationException ex)
            {
                return BadRequest(ex.Message);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [Authorize(Policy = UserClaims.TeacherDelete)]
        // DELETE api/<ValuesController>/5
        [HttpPost("deleteMany")]
        public async Task<ActionResult> DeleteTeachers(DeleteTeacher.Command command)
        {
            try
            {
                // TODO: not found handling //

                await mediator.Send(command);

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        private static bool IsInvalid(string firstname, string lastname)
        {
            return string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname);
        }
    }
}
