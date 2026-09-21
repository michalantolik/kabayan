using Kabayan.Application.Account.DeleteAccount;
using Kabayan.Application.Account.GetAccount;
using Kabayan.Application.Authentication.Login;
using Kabayan.Application.Authentication.Register;
using Microsoft.Extensions.DependencyInjection;

namespace Kabayan.Application;

/// <summary>
/// Registers Launchpad application services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<DeleteAccountHandler>();
        services.AddScoped<GetAccountHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<RegisterUserHandler>();

        return services;
    }
}
