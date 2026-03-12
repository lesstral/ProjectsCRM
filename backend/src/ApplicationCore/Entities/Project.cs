using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CompanyContractorName { get; set; }
        public string? CompanyExecutorName { get; set; }
        public int? ProjectManagerId { get; set; }
        public Employee? ProjectManager { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }

        //many-to-many
        public ICollection<Employee>? Employees { get; set; } = new List<Employee>();
        // one-to-many
        // public ICollection<ProjectTask>? Tasks { get; set; } = new List<ProjectTask>();
    }
}
