using Kabayan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Kabayan.Api.Tests.Fixtures;

public sealed class LaunchpadApiFactory
    : WebApplicationFactory<Program>
{
    public const string JwtIssuer =
        "Kabayan.Api.Tests";

    public const string JwtAudience =
        "Kabayan.Api.Tests.Client";

    public const string JwtSigningKey =
        "launchpad-api-tests-signing-key-32-chars-minimum";

    private readonly SqliteConnection _connection;
    private readonly Dictionary<string, string?> _originalEnvironmentVariables;

    public LaunchpadApiFactory()
    {
        _originalEnvironmentVariables =
            new Dictionary<string, string?>
            {
                ["Jwt__Issuer"] =
                    Environment.GetEnvironmentVariable("Jwt__Issuer"),
                ["Jwt__Audience"] =
                    Environment.GetEnvironmentVariable("Jwt__Audience"),
                ["Jwt__SigningKey"] =
                    Environment.GetEnvironmentVariable("Jwt__SigningKey"),
                ["Jwt__ExpirationInMinutes"] =
                    Environment.GetEnvironmentVariable("Jwt__ExpirationInMinutes")
            };

        Environment.SetEnvironmentVariable(
            "Jwt__Issuer",
            JwtIssuer);
        Environment.SetEnvironmentVariable(
            "Jwt__Audience",
            JwtAudience);
        Environment.SetEnvironmentVariable(
            "Jwt__SigningKey",
            JwtSigningKey);
        Environment.SetEnvironmentVariable(
            "Jwt__ExpirationInMinutes",
            "60");

        _connection =
            new SqliteConnection(
                "Data Source=:memory:");

        _connection.Open();
    }

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<KabayanDbContext>();
            services.RemoveAll<DbContextOptions<KabayanDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<KabayanDbContext>>();

            services.AddDbContext<KabayanDbContext>(options =>
                options.UseSqlite(_connection));

            using var serviceProvider =
                services.BuildServiceProvider();

            using var scope =
                serviceProvider.CreateScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<KabayanDbContext>();

            dbContext.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
        {
            return;
        }

        _connection.Dispose();

        foreach (var environmentVariable in
                 _originalEnvironmentVariables)
        {
            Environment.SetEnvironmentVariable(
                environmentVariable.Key,
                environmentVariable.Value);
        }
    }
}
