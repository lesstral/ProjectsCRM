using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.Employee
{
    public class EmployeeDTO
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }

        public List<int> AssignedProjectIds { get; set; } = new List<int>();
        public List<int> ManagedProjectIds { get; set; } = new List<int>();
    }
}
