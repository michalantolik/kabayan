using Kabayan.Application.Account;
using Kabayan.Application.Authentication;
using Kabayan.Application.Common.Identity;
using Kabayan.Infrastructure.Authentication.Jwt;
using Kabayan.Infrastructure.Common.Identity;
using Kabayan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kabayan.Infrastructure;

/// <summary>
/// Registers Launchpad infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Kabayan");

        services.AddDbContext<KabayanDbContext>(options =>
            options.UseSqlServer(connectionString));

        services
            .AddHealthChecks()
            .AddDbContextCheck<KabayanDbContext>();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<KabayanDbContext>();

        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IAccountService, AccountService>();

        services.Configure<JwtOptions>(
            configuration.GetSection(JwtOptions.SectionName));

        services.AddScoped<
            IAccessTokenGenerator,
            JwtAccessTokenGenerator>();

        return services;
    }
}
