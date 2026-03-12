using System.Net;
using System.Net.Http.Json;
using ApplicationCore.DTOs.Project;
using ApplicationCore.Entities;


namespace IntegrationTests;

public class ProjectIntegrationTests : IntegrationTestBase
{
    public ProjectIntegrationTests(PostgreFixture fixture) : base(fixture)
    {
        
    }
    [Fact]
    public async Task GetProject_ReturnsExpectedProject()
    {
        //arrange 
        int projectId = 1;
        //act
        var response = _client.GetAsync($"/api/project/{projectId}").Result;
        //assert
        Assert.True(response.IsSuccessStatusCode, 
            $"Expected success, got {(int)response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Сделать покушать", content);
    }
    [Fact]
    public async Task GetProject_NonExisting_ReturnsNotFound()
    {
        int projectId = 999; // does not exist
        
        var response = await _client.GetAsync($"/api/project/{projectId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task GetProject_WithNegativeProjectId_ReturnsBadRequest()
    {

        int negativeProjectId = -1;
        

        var response = await _client.GetAsync($"/api/project/{negativeProjectId}");
        

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    [Fact]
    public async Task CreateProject_WithValidData_ReturnsCreatedProject()
    {
        var newProject = new SaveProjectDTO
        {
            Name = "New Integration Test Project",
            CompanyContractorName = "Test Company",
            CompanyExecutorName =  "Test Company",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(3),
            Priority = 1,
            ProjectManagerId = 2,
            EmployeeIds = new List<int>()
        };
        

        var response = await _client.PostAsJsonAsync("/api/project", newProject);
        

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var createdProject = await response.Content.ReadFromJsonAsync<ProjectDTO>();
        
        Assert.NotNull(createdProject);
        Assert.Equal(newProject.Name, createdProject.Name);

    }
    [Fact]
    public async Task CreateProject_NonExistingEmployees_Returns404()
    {

        var newProject = new SaveProjectDTO
        {
            Name = "New Integration Test Project",
            CompanyContractorName = "Test Company",
            CompanyExecutorName =  "Test Company",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(3),
            Priority = 1,
            ProjectManagerId = 1,
            EmployeeIds = new List<int>{900, 500, 600}
        };
        

        var response = await _client.PostAsJsonAsync("/api/project", newProject);
        

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task CreateProject_NonExistingProjectManager_Returns404()
    {

        var newProject = new SaveProjectDTO
        {
            Name = "New Integration Test Project",
            CompanyContractorName = "Test Company",
            CompanyExecutorName =  "Test Company",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddMonths(3),
            Priority = 1,
            ProjectManagerId = 450,
            EmployeeIds = new List<int>{1, 2, 3}
        };
        

        var response = await _client.PostAsJsonAsync("/api/project", newProject);
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}