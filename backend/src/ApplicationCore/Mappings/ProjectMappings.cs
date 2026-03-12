using System.Xml.Linq;
using ApplicationCore.DTOs.Project;
using ApplicationCore.Entities;

namespace ApplicationCore.Mappings
{
    public static class ProjectMappings
    {
        public static Project ToEntity(SaveProjectDTO dto)
        {
            return new Project
            {
                Name = dto.Name,
                CompanyContractorName = dto.CompanyContractorName,
                CompanyExecutorName = dto.CompanyExecutorName,
                ProjectManagerId = dto.ProjectManagerId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Priority = dto.Priority,
            };
        }

        public static ProjectDTO ToDto(Project entity)
        {
            return new ProjectDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                CompanyContractorName = entity.CompanyContractorName,
                CompanyExecutorName = entity.CompanyExecutorName,
                ProjectManagerId = entity.ProjectManagerId,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Priority = entity.Priority,
                EmployeeIds = entity.Employees?.Select(e => e.Id).ToList() ?? new List<int>(),
            };
        }

        public static void UpdateEntity(Project entity, SaveProjectDTO dto)
        {
            entity.Name = dto.Name;
            entity.CompanyContractorName = dto.CompanyContractorName;
            entity.CompanyExecutorName = dto.CompanyExecutorName;
            entity.ProjectManagerId = dto.ProjectManagerId;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Priority = dto.Priority;
        }
    }
}
