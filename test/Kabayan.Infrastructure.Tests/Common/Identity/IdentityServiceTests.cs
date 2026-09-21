using Kabayan.Application.Authentication.Register;
using Kabayan.Infrastructure.Common.Identity;
using Kabayan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kabayan.Infrastructure.Tests.Common.Identity;

public sealed class IdentityServiceTests
{
    private const string ValidPassword =
        "ValidPassword1!";

    [Fact]
    public async Task CreateUserAsync_ValidUser_CreatesUser()
    {
        await using var context =
            await IdentityTestContext.CreateAsync();

        const string email = "user@example.com";

        var userId = await context.IdentityService.CreateUserAsync(
            email,
            ValidPassword);

        var user = await context.UserManager.FindByEmailAsync(email);

        Assert.NotEqual(
            Guid.Empty,
            userId);
        Assert.NotNull(user);
        Assert.Equal(
            userId,
            user.Id);
        Assert.Equal(
            email,
            user.Email);
        Assert.True(
            await context.UserManager.CheckPasswordAsync(
                user,
                ValidPassword));
    }

    [Fact]
    public async Task CreateUserAsync_DuplicateEmail_ThrowsUserRegistrationException()
    {
        await using var context =
            await IdentityTestContext.CreateAsync();

        const string email = "user@example.com";

        await context.IdentityService.CreateUserAsync(
            email,
            ValidPassword);

        var exception = await Assert.ThrowsAsync<UserRegistrationException>(
            () => context.IdentityService.CreateUserAsync(
                email,
                ValidPassword));

        Assert.NotEmpty(exception.Errors);
    }

    [Fact]
    public async Task AuthenticateUserAsync_ValidCredentials_ReturnsUserId()
    {
        await using var context =
            await IdentityTestContext.CreateAsync();

        const string email = "user@example.com";

        var expectedUserId =
            await context.IdentityService.CreateUserAsync(
                email,
                ValidPassword);

        var userId =
            await context.IdentityService.AuthenticateUserAsync(
                email,
                ValidPassword);

        Assert.Equal(
            expectedUserId,
            userId);
    }

    [Fact]
    public async Task AuthenticateUserAsync_InvalidPassword_ReturnsNull()
    {
        await using var context =
            await IdentityTestContext.CreateAsync();

        const string email = "user@example.com";

        await context.IdentityService.CreateUserAsync(
            email,
            ValidPassword);

        var userId =
            await context.IdentityService.AuthenticateUserAsync(
                email,
                "WrongPassword1!");

        Assert.Null(userId);
    }

    private sealed class IdentityTestContext
        : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ServiceProvider _serviceProvider;
        private readonly AsyncServiceScope _scope;

        private IdentityTestContext(
            SqliteConnection connection,
            ServiceProvider serviceProvider,
            AsyncServiceScope scope,
            UserManager<ApplicationUser> userManager)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _scope = scope;
            UserManager = userManager;
            IdentityService = new IdentityService(userManager);
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public IdentityService IdentityService { get; }

        public static async Task<IdentityTestContext> CreateAsync()
        {
            var connection =
                new SqliteConnection(
                    "Data Source=:memory:");

            await connection.OpenAsync();

            var services =
                new ServiceCollection();

            services.AddLogging();

            services.AddDbContext<KabayanDbContext>(options =>
                options.UseSqlite(connection));

            services
                .AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<KabayanDbContext>();

            var serviceProvider =
                services.BuildServiceProvider();

            var scope =
                serviceProvider.CreateAsyncScope();

            var dbContext =
                scope.ServiceProvider
                    .GetRequiredService<KabayanDbContext>();

            await dbContext.Database.EnsureCreatedAsync();

            var userManager =
                scope.ServiceProvider
                    .GetRequiredService<UserManager<ApplicationUser>>();

            return new IdentityTestContext(
                connection,
                serviceProvider,
                scope,
                userManager);
        }

        public async ValueTask DisposeAsync()
        {
            await _scope.DisposeAsync();
            await _serviceProvider.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
