using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.DTOs.Project
{
    public class ProjectDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? CompanyContractorName { get; set; }

        public string? CompanyExecutorName { get; set; }
        public int? ProjectManagerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Priority { get; set; }
        public List<int> EmployeeIds { get; set; } = new List<int>();
    }
}
