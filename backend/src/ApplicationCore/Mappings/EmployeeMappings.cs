using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DTOs.Employee;
using ApplicationCore.Entities;

namespace ApplicationCore.Mappings
{
    public static class EmployeeMappings
    {
        public static Employee ToEntity(SaveEmployeeDTO dto)
        {
            return new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddleName = dto.MiddleName,
                Email = dto.Email,
            };
        }

        public static EmployeeDTO ToDto(Employee entity)
        {
            return new EmployeeDTO
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                MiddleName = entity.MiddleName,
                Email = entity.Email,
                ManagedProjectIds =
                    entity.ManagedProjects?.Select(p => p.Id).ToList() ?? new List<int>(),
                AssignedProjectIds = entity.Projects?.Select(p => p.Id).ToList() ?? new List<int>(),
            };
        }

        public static void UpdateEntity(Employee entity, SaveEmployeeDTO dto)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.MiddleName = dto.MiddleName;
            entity.Email = dto.Email;
        }
    }
}
