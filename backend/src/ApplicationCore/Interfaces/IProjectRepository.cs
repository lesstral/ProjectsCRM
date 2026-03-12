using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Project;

namespace ApplicationCore.Interfaces
{
    public interface IProjectRepository
    {
        public Task<ProjectDTO> CreateProjectAsync(SaveProjectDTO dto);
        public Task<ProjectDTO?> UpdateProjectAsync(int id, SaveProjectDTO dto);
        public Task<bool> DeleteProjectAsync(int id);
        public Task<ProjectDTO?> GetProjectByIdAsync(int id);
        public Task<ListDTO<ProjectDTO>> GetProjectsListAsync(QueryDTO dto);
    }
}
