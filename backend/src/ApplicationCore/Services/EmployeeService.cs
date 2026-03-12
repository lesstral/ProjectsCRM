using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Employee;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Services;

public class EmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<EmployeeDTO>> CreateAsync(SaveEmployeeDTO dto)
    {
        var result = await _repository.CreateEmployeeAsync(dto);
        return Result<EmployeeDTO>.Success(result);
    }

    public async Task<Result<EmployeeDTO>> UpdateAsync(int id, SaveEmployeeDTO dto)
    {
        var result = await _repository.UpdateEmployeeAsync(id, dto);
        if (result == null)
            return Result<EmployeeDTO>.Failure(
                $"Failed to update employee with id: {id}",
                ErrorType.NotFound
            );

        return Result<EmployeeDTO>.Success(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var result = await _repository.DeleteEmployeeAsync(id);
        if (!result)
        {
            return Result<bool>.Failure(
                $"Failed to delete employee with id: {id}",
                ErrorType.NotFound
            );
        }

        return Result<bool>.Success(result);
    }

    public async Task<Result<EmployeeDTO>> GetEmployeeAsync(int id)
    {
        var result = await _repository.GetEmployeeByIdAsync(id);
        if (result == null)
        {
            return Result<EmployeeDTO>.Failure(
                $"Failed to get employee with id: {id}",
                ErrorType.NotFound
            );
        }

        return Result<EmployeeDTO>.Success(result);
    }

    public async Task<Result<ListDTO<EmployeeDTO>>> GetEmployeesListAsync(QueryDTO query)
    {
        if (query.Page < 1)
        {
            return Result<ListDTO<EmployeeDTO>>.Failure(
                "Page cannot be less than 1",
                ErrorType.Validation
            );
        }

        if (query.ItemsPerPage < 1)
        {
            return Result<ListDTO<EmployeeDTO>>.Failure(
                "Items per page must be greater than 0",
                ErrorType.Validation
            );
        }

        var result = await _repository.GetEmployeesListAsync(query);
        return Result<ListDTO<EmployeeDTO>>.Success(result);
    }

    public async Task<Result<List<FullNameEmployeeDTO>>> SearchAsync(string query, int limit)
    {
        var result = await _repository.SearchAsync(query, limit);
        return Result<List<FullNameEmployeeDTO>>.Success(result);
    }

    public async Task<Result<FullNameEmployeeDTO>> GetFullNameAsync(int id)
    {
        var result = await _repository.GetFullNameAsync(id);
        if (result == null)
        {
            return Result<FullNameEmployeeDTO>.Failure(
                $"Failed to get employee with id: {id}",
                ErrorType.NotFound
            );
        }

        return Result<FullNameEmployeeDTO>.Success(result);
    }
}
