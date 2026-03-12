using System.Data.Common;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory<TProgram> : 
    WebApplicationFactory<TProgram> where TProgram : class
{
    private string? _connectionString;

    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new InvalidOperationException("Connection string not set.");
        }
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == 
                     typeof(DbContextOptions<AppDbContext>));

            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbConnection));

            services.Remove(dbConnectionDescriptor);
            
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(_connectionString));

        });
        builder.ConfigureServices(services =>
        {
            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.Migrate();
            DbSeeder.SeedAsync(db).GetAwaiter().GetResult();
        });
    }
}