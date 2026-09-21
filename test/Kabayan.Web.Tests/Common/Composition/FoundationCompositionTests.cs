using Kabayan.Web.Common.Composition;
using Kabayan.Web.Tests.Fixtures;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Kabayan.Web.Tests.Common.Composition;

public sealed class FoundationCompositionTests(
    LaunchpadWebFactory factory)
    : IClassFixture<LaunchpadWebFactory>
{
    [Fact]
    public void WithoutLocalization_ReturnsEnglishFallback()
    {
        Assert.Equal(
            "Launchpad — Sign in",
            ProductCapabilities.Text(
                "Login.PageTitle",
                "Launchpad — Sign in"));
        Assert.Equal(
            "Account created",
            ProductCapabilities.Text(
                "Register.CreatedTitle",
                "Account created"));
    }

    [Fact]
    public async Task LoginPage_RendersEnglishFallback()
    {
        using var client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost")
            });

        var html = System.Net.WebUtility.HtmlDecode(
            await client.GetStringAsync("/login"));

        Assert.Contains("— Sign in</title>", html);
    }
}
