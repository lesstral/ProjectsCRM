using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Project;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(SaveProjectDTO dto)
        {
            if (dto.ProjectManagerId < 1 || dto.EmployeeIds.Any(id => id < 1))
                return BadRequest("Not a valid id");

            //convert to utc
            dto.StartDate = dto.StartDate.ToUniversalTime();
            dto.EndDate = dto.EndDate.ToUniversalTime();
            var result = await _projectService.CreateAsync(dto);
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute] int id,
            [FromBody] SaveProjectDTO dto
        )
        {
            if (id < 1 || dto.ProjectManagerId < 1 || dto.EmployeeIds.Any(id => id < 1))
                return BadRequest("Not a valid id");
            //convert to utc
            dto.StartDate = dto.StartDate.ToUniversalTime();
            dto.EndDate = dto.EndDate.ToUniversalTime();

            var result = await _projectService.UpdateAsync(id, dto);
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id < 1)
                return BadRequest("Not a valid id");

            var result = await _projectService.DeleteAsync(id);
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            if (id < 1)
                return BadRequest("Not a valid id");
            var result = await _projectService.GetProjectAsnyc(id);
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetProjectsAsync([FromQuery] QueryDTO query)
        {
            var result = await _projectService.GetProjectsListAsync(query);
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }
    }
}
