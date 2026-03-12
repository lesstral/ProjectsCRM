namespace IntegrationTests;

public class IntegrationTestBase : IClassFixture<PostgreFixture>
{
    protected readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly PostgreFixture _fixture;

    public IntegrationTestBase(PostgreFixture fixture)
    {
        _fixture = fixture;
        _factory = new CustomWebApplicationFactory<Program>(fixture.ConnectionString);
        _client = _factory.CreateClient();
    }
}
