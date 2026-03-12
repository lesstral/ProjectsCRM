using ApplicationCore.DTOs;
using ApplicationCore.DTOs.Project;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using FluentAssertions;
using Moq;

namespace ApplicationCore.UnitTests
{
    public class ProjectTests
    {
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<IEmployeeRepository> _mockEmployeeRepository;
        private readonly ProjectService _projectService;
        
        public ProjectTests()
        {
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockEmployeeRepository = new Mock<IEmployeeRepository>();
            _projectService = new ProjectService(_mockProjectRepository.Object, _mockEmployeeRepository.Object);
        }
        #region CreateAsync Tests
        [Fact]
        public async Task CreateProject_EndDateInThePast_ShouldFail()
        {
            var testDto = new SaveProjectDTO
            {
                Name = "Test",
                Priority = 1,
                CompanyContractorName = "Test LLC",
                CompanyExecutorName = "Test LLC",
                StartDate = DateTime.Today.AddDays(-15).ToUniversalTime(),
                EndDate = DateTime.Today.AddDays(-5).ToUniversalTime(),
            };
            var result = await _projectService.CreateAsync(testDto);
            result.IsSuccess.Should().BeFalse();
        }
        [Fact]
        public async Task CreateProject_EndBeforeStartDate_ShouldFail()
        {
            var testDto = new SaveProjectDTO
            {
                Name = "Test",
                Priority = 1,
                CompanyContractorName = "Test LLC",
                CompanyExecutorName = "Test LLC",
                StartDate = DateTime.Today.AddDays(-1).ToUniversalTime(),
                EndDate = DateTime.Today.AddDays(-5).ToUniversalTime(),
            };
            var result = await _projectService.CreateAsync(testDto);
            result.IsSuccess.Should().BeFalse();
        }
        
        #endregion
        #region UpdateAsync Tests
        [Fact]
        public async Task UpdateAsync_ReturnsNull_ShouldFail()
        {

            var testDto = new SaveProjectDTO
            {
                Name = "Valid Project",
                Priority = 1,
                CompanyContractorName = "Contractor LLC",
                CompanyExecutorName = "Executor LLC",
                StartDate = DateTime.Today.ToUniversalTime(),
                EndDate = DateTime.Today.AddMonths(3).ToUniversalTime(),
            };

            _mockProjectRepository.Setup(x => x.UpdateProjectAsync(1, It.IsAny<SaveProjectDTO>()))
                .ReturnsAsync(default(ProjectDTO));


            var result = await _projectService.UpdateAsync(1, testDto);


            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Failed to update project");
        }
        [Fact]
        public async Task UpdateAsync_NonExistingEmployees_ShouldFail()
        {

            var testDto = new SaveProjectDTO
            {
                Name = "Valid Project",
                Priority = 1,
                CompanyContractorName = "Contractor LLC",
                CompanyExecutorName = "Executor LLC",
                EmployeeIds = [450, 120], 
                StartDate = DateTime.Today.ToUniversalTime(),
                EndDate = DateTime.Today.AddMonths(3).ToUniversalTime(),
            };

            _mockEmployeeRepository.Setup(x => 
                x.GetMissingIds(testDto.EmployeeIds)).ReturnsAsync(testDto.EmployeeIds);


            var result = await _projectService.UpdateAsync(1, testDto);


            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("450,120");
        }
        #endregion
        #region GetProjectsListAsync Tests

        [Fact]
        public async Task GetProjectListAsync_InvalidPage_ShouldFail()
        {
            var invalidPageQuery = new QueryDTO
            {
                Page = -1,
                ItemsPerPage = 5
            };

            var result = await _projectService.GetProjectsListAsync(invalidPageQuery);
            
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Page");
        }
        
        [Fact]
        public async Task GetProjectListAsync_InvalidItemsPerPage_ShouldFail()
        {
            var invalidItemsPerPageQuery = new QueryDTO
            {
                Page = 1,
                ItemsPerPage = -5
            };

            var result = await _projectService.GetProjectsListAsync(invalidItemsPerPageQuery);
            
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Contain("Items");
        }
        #endregion
        
    }
}
