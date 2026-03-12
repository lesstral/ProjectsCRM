using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }

        //many-to-many
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        // one-to-many
        //public ICollection<ProjectTask> TasksAuthored { get; set; } = new List<ProjectTask>();
        //public ICollection<ProjectTask> TasksAssigned { get; set; } = new List<ProjectTask>();
        public ICollection<Project> ManagedProjects { get; set; } = new List<Project>();
    }
}
