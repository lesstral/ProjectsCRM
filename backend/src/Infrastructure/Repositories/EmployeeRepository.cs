using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Employee;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployeeDTO> CreateEmployeeAsync(SaveEmployeeDTO dto)
        {
            //add assigned projects
            var entity = EmployeeMappings.ToEntity(dto);
            await _context.Employees.AddAsync(entity);
            await _context.SaveChangesAsync();

            return EmployeeMappings.ToDto(entity);
        }

        public async Task<EmployeeDTO> UpdateEmployeeAsync(int id, SaveEmployeeDTO dto)
        {
            //check if exists
            var entity = await _context
                .Employees.Include(e => e.Projects)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
                return null;
            EmployeeMappings.UpdateEntity(entity, dto);

            await _context.SaveChangesAsync();
            return EmployeeMappings.ToDto(entity);
        }

        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null)
            {
                return false;
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<int>> GetMissingIds(List<int> ids)
        {
            var existingIds = await _context
                .Employees.Where(e => ids.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync();

            return ids.Except(existingIds).ToList();
        }

        public async Task<EmployeeDTO?> GetEmployeeByIdAsync(int id)
        {
            //add assigned projects
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null)
                return null;
            return EmployeeMappings.ToDto(entity);
        }

        public async Task<ListDTO<EmployeeDTO>> GetEmployeesListAsync(QueryDTO dto)
        {
            IQueryable<Employee> entity = _context
                .Employees.AsNoTracking()
                .Include(p => p.Projects)
                .Include(p => p.ManagedProjects);

            var totalCount = await entity.CountAsync();
            // determine what to sort by

            entity = dto.SortBy?.ToLower() switch
            {
                "firstname" => dto.Descending
                    ? entity.OrderByDescending(p => p.FirstName)
                    : entity.OrderBy(p => p.FirstName),

                "lastname" => dto.Descending
                    ? entity.OrderByDescending(p => p.LastName)
                    : entity.OrderBy(p => p.LastName),

                "middlename" => dto.Descending
                    ? entity.OrderByDescending(p => p.MiddleName)
                    : entity.OrderBy(p => p.MiddleName),

                "email" => dto.Descending
                    ? entity.OrderByDescending(p => p.Email)
                    : entity.OrderBy(p => p.Email),
                _ => entity.OrderBy(p => p.Id),
            };
            // get pagination and fetch

            var items = await entity
                .Skip((dto.Page - 1) * dto.ItemsPerPage)
                .Take(dto.ItemsPerPage)
                .ToListAsync();
            return new ListDTO<EmployeeDTO>
            {
                TotalItemCount = totalCount,
                Items = items.Select(EmployeeMappings.ToDto).ToList(),
            };
        }

        public async Task<List<FullNameEmployeeDTO>> SearchAsync(string query, int itemLimit)
        {
            query = query.Normalize();
            IQueryable<FullNameEmployeeDTO> queryable = _context
                .Employees.AsNoTracking()
                .Where(e =>
                    e.FirstName.StartsWith(query)
                    || e.MiddleName.StartsWith(query)
                    || e.LastName.StartsWith(query)
                )
                .Take(itemLimit)
                .Select(e => new FullNameEmployeeDTO
                {
                    Id = e.Id,
                    FullName = $"{e.LastName} {e.FirstName} {e.MiddleName}",
                });
            var result = await queryable.ToListAsync();

            return result;
        }

        public async Task<FullNameEmployeeDTO?> GetFullNameAsync(int id)
        {
            var entity = await _context.Employees.FindAsync(id);
            if (entity == null)
                return null;

            var dto = new FullNameEmployeeDTO
            {
                Id = entity.Id,
                FullName = $"{entity.LastName} {entity.FirstName} {entity.MiddleName}",
            };

            return dto;
        }
    }
}
