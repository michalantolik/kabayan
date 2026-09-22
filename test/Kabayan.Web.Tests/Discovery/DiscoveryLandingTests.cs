using Kabayan.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Kabayan.Web.Tests.Discovery;

public sealed class DiscoveryLandingTests(
    LaunchpadWebFactory factory)
    : IClassFixture<LaunchpadWebFactory>
{
    [Fact]
    public async Task Landing_AnonymousVisitor_SeesDiscoveryExperience()
    {
        using var client = CreateClient();

        using var response = await client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains(
            "Let’s make life in Poland easier—starting by listening.",
            content);
        Assert.Contains(
            "Survey status: not open yet",
            content);
        Assert.DoesNotContain(
            "Reusable SaaS foundation.",
            content);
    }

    [Theory]
    [InlineData("/discovery-landing.css", "text/css")]
    [InlineData("/discovery-landing.js", "text/javascript")]
    public async Task LandingAsset_Requested_ReturnsExpectedContentType(
        string path,
        string contentType)
    {
        using var client = CreateClient();

        using var response = await client.GetAsync(path);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.StartsWith(
            contentType,
            response.Content.Headers.ContentType?.MediaType);
    }

    private HttpClient CreateClient() =>
        factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost")
            });
}
