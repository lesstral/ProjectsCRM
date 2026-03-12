using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Employee;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(SaveEmployeeDTO dto)
        {
            var result = await _employeeService.CreateAsync(dto);
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
            [FromBody] SaveEmployeeDTO dto
        )
        {
            if (id < 0)
                return BadRequest("Not a valid id");

            var result = await _employeeService.UpdateAsync(id, dto);
            Console.WriteLine(dto.ToString());
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            if (id < 0)
                return BadRequest("Not a valid id");

            var result = await _employeeService.DeleteAsync(id);

            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            if (id < 0)
                return BadRequest("Not a valid id");

            var result = await _employeeService.GetEmployeeAsync(id);

            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetEmployeesAsync([FromQuery] QueryDTO query)
        {
            var result = await _employeeService.GetEmployeesListAsync(query);
            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync(
            [FromQuery] string query,
            [FromQuery] int itemLimit = 10
        )
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Query string is empty");

            var result = await _employeeService.SearchAsync(query, itemLimit);

            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }

        [HttpGet("fullname")]
        public async Task<IActionResult> GetFullNameAsync([FromQuery] int id)
        {
            if (id < 0)
                return BadRequest("Not a valid id");

            var result = await _employeeService.GetFullNameAsync(id);

            if (!result.IsSuccess)
                return StatusCode(
                    (int)ErrorMapper.ToStatusCode(result.ErrorType),
                    result.ErrorMessage
                );

            return Ok(result.Value);
        }
    }
}
