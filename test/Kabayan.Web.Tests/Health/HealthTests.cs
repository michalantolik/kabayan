using Kabayan.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Kabayan.Web.Tests.Health;

public sealed class HealthTests(
    LaunchpadWebFactory factory)
    : IClassFixture<LaunchpadWebFactory>
{
    [Fact]
    public async Task Health_HostAvailable_ReturnsOk()
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
