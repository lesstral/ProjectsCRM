using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Project;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectDTO> CreateProjectAsync(SaveProjectDTO dto)
        {
            var entity = ProjectMappings.ToEntity(dto);
            await _context.Projects.AddAsync(entity);

            if (dto.EmployeeIds?.Any() == true)
            {
                var employees = await _context
                    .Employees.Where(e => dto.EmployeeIds.Contains(e.Id))
                    .ToListAsync();

                foreach (var employee in employees)
                {
                    entity.Employees.Add(employee);
                }
            }

            await _context.SaveChangesAsync();
            return ProjectMappings.ToDto(entity);
        }

        public async Task<ProjectDTO?> UpdateProjectAsync(int id, SaveProjectDTO dto)
        {
            var entity = await _context
                .Projects.Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return null;

            ProjectMappings.UpdateEntity(entity, dto);
            //clear relations
            entity.Id = id;
            entity.Employees.Clear();

            if (dto.EmployeeIds != null)
            {
                // add new relations + existing
                var employees = await _context
                    .Employees.Where(e => dto.EmployeeIds.Contains(e.Id))
                    .ToListAsync();

                foreach (var employee in employees)
                    entity.Employees.Add(employee);
            }

            await _context.SaveChangesAsync();
            return ProjectMappings.ToDto(entity);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var entity = await _context.Projects.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id)
        {
            var entity = await _context
                .Projects.Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null)
                return null;

            var dto = ProjectMappings.ToDto(entity);

            return dto;
        }

        public async Task<ListDTO<ProjectDTO>> GetProjectsListAsync(QueryDTO dto)
        {
            IQueryable<Project> entity = _context.Projects.AsNoTracking().Include(p => p.Employees);

            var totalCount = await entity.CountAsync();
            // determine what to sort by

            entity = dto.SortBy?.ToLower() switch
            {
                "name" => dto.Descending
                    ? entity.OrderByDescending(p => p.Name)
                    : entity.OrderBy(p => p.Name),

                "startdate" => dto.Descending
                    ? entity.OrderByDescending(p => p.StartDate)
                    : entity.OrderBy(p => p.StartDate),

                "enddate" => dto.Descending
                    ? entity.OrderByDescending(p => p.EndDate)
                    : entity.OrderBy(p => p.EndDate),

                "priority" => dto.Descending
                    ? entity.OrderByDescending(p => p.Priority)
                    : entity.OrderBy(p => p.Priority),
                "companycontractorname" => dto.Descending
                    ? entity.OrderByDescending(p => p.CompanyContractorName)
                    : entity.OrderBy(p => p.CompanyContractorName),
                "companyexecutorname" => dto.Descending
                    ? entity.OrderByDescending(p => p.CompanyExecutorName)
                    : entity.OrderBy(p => p.CompanyExecutorName),

                _ => entity.OrderBy(p => p.Id),
            };
            // get pagination and fetch

            var items = await entity
                .Skip((dto.Page - 1) * dto.ItemsPerPage)
                .Take(dto.ItemsPerPage)
                .ToListAsync();

            return new ListDTO<ProjectDTO>
            {
                TotalItemCount = totalCount,
                Items = items.Select(ProjectMappings.ToDto).ToList(),
            };
        }
    }
}
