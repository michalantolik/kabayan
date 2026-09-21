namespace Kabayan.Web.Common.Composition;

public static class ProductCapabilities
{
    public static void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration)
    {
    }

    public static void ConfigurePipeline(
        WebApplication app)
    {
    }

    public static string Text(
        string key,
        string fallback) =>
        fallback;
}
