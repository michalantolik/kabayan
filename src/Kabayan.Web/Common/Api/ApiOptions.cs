namespace Kabayan.Web.Common.Api;

/// <summary>
/// Configuration for the Launchpad API used by the Web host.
/// </summary>
public sealed class ApiOptions
{
    public const string SectionName = "Api";

    public required string BaseUrl { get; init; }
}
