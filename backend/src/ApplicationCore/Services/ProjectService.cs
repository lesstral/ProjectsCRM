using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Project;
using ApplicationCore.Enums;
using ApplicationCore.Interfaces;

namespace ApplicationCore.Services;

public class ProjectService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IEmployeeRepository _employeeRepository;

    public ProjectService(
        IProjectRepository projectRepository,
        IEmployeeRepository employeeRepository
    )
    {
        _projectRepository = projectRepository;
        _employeeRepository = employeeRepository;
    }

    public async Task<Result<ProjectDTO>> CreateAsync(SaveProjectDTO dto)
    {
        if (dto.StartDate >= dto.EndDate)
            return Result<ProjectDTO>.Failure(
                "Start date cannot be after end date",
                ErrorType.Validation
            );
        if (dto.EndDate <= DateTime.Today.ToUniversalTime())
            return Result<ProjectDTO>.Failure(
                "End date cannot be in the past",
                ErrorType.Validation
            );

        if (dto.ProjectManagerId != null || dto.EmployeeIds.Any())
        {
            var checkResult = await ValidateEmployeesExist(dto.EmployeeIds, dto.ProjectManagerId);
            if (!checkResult.IsSuccess)
                return Result<ProjectDTO>.Failure(checkResult.ErrorMessage, checkResult.ErrorType);
        }

        var result = await _projectRepository.CreateProjectAsync(dto);
        return Result<ProjectDTO>.Success(result);
    }

    public async Task<Result<ProjectDTO>> UpdateAsync(int id, SaveProjectDTO dto)
    {
        if (dto.StartDate >= dto.EndDate)
            return Result<ProjectDTO>.Failure(
                "Start date cannot be after end date",
                ErrorType.Validation
            );
        if (dto.EndDate <= DateTime.Today.ToUniversalTime())
            return Result<ProjectDTO>.Failure(
                "End date cannot be in the past",
                ErrorType.Validation
            );

        if (dto.ProjectManagerId != null || dto.EmployeeIds.Any())
        {
            var checkResult = await ValidateEmployeesExist(dto.EmployeeIds, dto.ProjectManagerId);
            if (!checkResult.IsSuccess)
                return Result<ProjectDTO>.Failure(checkResult.ErrorMessage, checkResult.ErrorType);
        }
        var result = await _projectRepository.UpdateProjectAsync(id, dto);
        if (result == null)
            return Result<ProjectDTO>.Failure(
                $"Failed to update project with id: {id}",
                ErrorType.NotFound
            );

        return Result<ProjectDTO>.Success(result);
    }

    public async Task<Result<bool>> DeleteAsync(int id)
    {
        var result = await _projectRepository.DeleteProjectAsync(id);
        if (!result)
        {
            return Result<bool>.Failure(
                $"Failed to delete project with id: {id}",
                ErrorType.NotFound
            );
        }

        return Result<bool>.Success(result);
    }

    public async Task<Result<ProjectDTO>> GetProjectAsnyc(int id)
    {
        var result = await _projectRepository.GetProjectByIdAsync(id);
        if (result == null)
        {
            return Result<ProjectDTO>.Failure(
                $"Failed to get project with id: {id}",
                ErrorType.NotFound
            );
        }

        return Result<ProjectDTO>.Success(result);
    }

    public async Task<Result<ListDTO<ProjectDTO>>> GetProjectsListAsync(QueryDTO query)
    {
        if (query.Page < 1)
            return Result<ListDTO<ProjectDTO>>.Failure(
                "Page cannot be less than 1",
                ErrorType.Validation
            );

        if (query.ItemsPerPage < 1)
            return Result<ListDTO<ProjectDTO>>.Failure(
                "Items per page must be greater than 0",
                ErrorType.Validation
            );

        var result = await _projectRepository.GetProjectsListAsync(query);

        return Result<ListDTO<ProjectDTO>>.Success(result);
    }

    private async Task<Result> ValidateEmployeesExist(List<int> employeeIds, int? projectManagerId)
    {
        var combinedIds = new List<int>(employeeIds);
        if (projectManagerId != null && !combinedIds.Contains(projectManagerId.Value))
        {
            combinedIds.Add(projectManagerId.Value);
        }

        var missingIds = await _employeeRepository.GetMissingIds(combinedIds);
        if (missingIds.Any())
        {
            return Result.Failure(
                $"Missing ids: {string.Join(",", missingIds)}",
                ErrorType.NotFound
            );
        }

        return Result.Success();
    }
}
