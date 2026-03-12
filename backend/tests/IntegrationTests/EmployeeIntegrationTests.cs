using System.Net;
using System.Net.Http.Json;
using ApplicationCore.DTOs.Employee;

namespace IntegrationTests;

public class EmployeeIntegrationTests : IntegrationTestBase
{
    public EmployeeIntegrationTests(PostgreFixture fixture)
        : base(fixture) { }

    [Fact]
    public async Task GetEmployee_ReturnsExpectedEmployee()
    {
        //arrange
        int employeeId = 1;
        //act
        var response = _client.GetAsync($"/api/employee/{employeeId}").Result;
        //assert
        Assert.True(
            response.IsSuccessStatusCode,
            $"Expected success, got {(int)response.StatusCode}"
        );
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Ivan", content);
    }

    [Fact]
    public async Task GetEmployee_NonExisting_ReturnsNotFound()
    {
        int employeeId = 999; // does not exist

        var response = await _client.GetAsync($"/api/employee/{employeeId}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployee_WithNegativeEmployeeId_ReturnsBadRequest()
    {
        int negativeEmployeeId = -1;

        var response = await _client.GetAsync($"/api/employee/{negativeEmployeeId}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateEmployee_WithValidData_ReturnsCreatedEmployee()
    {
        var newEmployee = new EmployeeDTO
        {
            FirstName = "Test",
            LastName = "Testov",
            MiddleName = "Testovich",
            Email = "test@test.test",
        };
        var response = await _client.PostAsJsonAsync("/api/employee", newEmployee);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var createdEmployee = await response.Content.ReadFromJsonAsync<EmployeeDTO>();
        Assert.NotNull(createdEmployee);
        Assert.Equal(newEmployee.FirstName, createdEmployee.FirstName);
    }
}
