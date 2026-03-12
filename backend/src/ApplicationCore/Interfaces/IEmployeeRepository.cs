using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Employee;

namespace ApplicationCore.Interfaces
{
    public interface IEmployeeRepository
    {
        public Task<EmployeeDTO> CreateEmployeeAsync(SaveEmployeeDTO dto);
        public Task<EmployeeDTO?> UpdateEmployeeAsync(int id, SaveEmployeeDTO dto);
        public Task<bool> DeleteEmployeeAsync(int id);
        public Task<List<int>> GetMissingIds(List<int> ids);
        public Task<EmployeeDTO?> GetEmployeeByIdAsync(int id);
        public Task<ListDTO<EmployeeDTO>> GetEmployeesListAsync(QueryDTO query);
        public Task<List<FullNameEmployeeDTO>> SearchAsync(string query, int itemLimit);
        public Task<FullNameEmployeeDTO?> GetFullNameAsync(int id);
    }
}
