using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DTOs.Project
{
    public class SaveProjectDTO
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(250)]
        public string CompanyContractorName { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(250)]
        public string CompanyExecutorName { get; set; }
        public int? ProjectManagerId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(1, 10)]
        public int Priority { get; set; }
        public List<int> EmployeeIds { get; set; } = new List<int>();
    }
}
