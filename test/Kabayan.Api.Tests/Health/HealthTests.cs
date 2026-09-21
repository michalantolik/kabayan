using Kabayan.Api.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Kabayan.Api.Tests.Health;

public sealed class HealthTests(
    LaunchpadApiFactory factory)
    : IClassFixture<LaunchpadApiFactory>
{
    [Fact]
    public async Task Health_DatabaseAvailable_ReturnsOk()
    {
        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });

        var response = await client.GetAsync("/health");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }
}
