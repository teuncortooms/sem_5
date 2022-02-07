using Core.Application.QueryUtil;
using Core.Application.Students.Commands;
using Core.Application.Students.Models;
using Core.Application.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Core.Domain.Entities;
using IdentityService;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ILogger<StudentsController> logger;

        public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [Authorize(Policy = UserClaims.StudentReadAll)]
        public async Task<ActionResult<PagedListWrapper<StudentModel>>> GetAll(int page, int pageSize, string filter, string orderBy)
        {
            try
            {
                var response = await mediator.Send(new GetAllStudents.Query()
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

        [HttpGet("getMyDetails")]
        [Authorize(Policy = UserClaims.StudentReadOwn)]
        public async Task<ActionResult<StudentDetailsModel>> GetMyDetails()
        {
            try
            {
                var claimValue = User.Claims.FirstOrDefault(c => c.Type == UserClaims.StudentId)?.Value;
                if (claimValue == null) return BadRequest("No valid id found");

                Guid id = new (claimValue);

                return await this.Get(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = UserClaims.StudentReadOwn)]
        public async Task<ActionResult<StudentDetailsModel>> Get(Guid id)
        {
            try
            {
                StudentDetailsModel student = await mediator.Send(new GetStudentDetails.Query(id));

                if (student == null)
                {
                    logger.LogDebug("Not found: " + id);
                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [HttpPost]
        [Authorize(Policy = UserClaims.StudentWrite)]
        public async Task<ActionResult<AddStudentModel>> AddStudent([FromBody] AddStudent.Command command)
        {
            try
            {
                if (IsInvalid(command.FirstName, command.LastName))
                {
                    return BadRequest("Check if all properties are set.");
                }

                AddStudentModel response = await mediator.Send(command);

                if (response == null)
                {
                    logger.LogDebug("Could not add student");
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

        private static bool IsInvalid(string firstname, string lastname)
        {
            return string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(lastname);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = UserClaims.StudentWrite)]
        public async Task<ActionResult> DeleteStudent(Guid id)
        {
            try
            {
                // TODO: not found handling //

                await mediator.Send(new DeleteStudents.Command(id));

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }

        [HttpPost("deleteMany")]
        [Authorize(Policy = UserClaims.StudentWrite)]
        public async Task<ActionResult> DeleteStudents(DeleteStudents.Command command)
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

        [HttpPut("{id}")]
        [Authorize(Policy = UserClaims.StudentWrite)]
        public async Task<ActionResult<UpdateStudentModel>> UpdateStudent(Guid id, [FromBody] UpdateStudent.Command command)
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
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception occurred");
                return Problem("An error occurred");
            }
        }
    }
}
