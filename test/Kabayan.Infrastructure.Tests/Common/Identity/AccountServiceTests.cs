using Kabayan.Application.Account;
using Kabayan.Infrastructure.Common.Identity;
using Kabayan.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Kabayan.Infrastructure.Tests.Identity;

public sealed class AccountServiceTests
{
    private const string ValidPassword =
        "ValidPassword1!";

    [Fact]
    public async Task GetAccountAsync_ExistingUser_ReturnsAccount()
    {
        await using var context =
            await AccountTestContext.CreateAsync();

        const string email = "user@example.com";

        var userId = await context.CreateUserAsync(
            email);

        var account =
            await context.AccountService.GetAccountAsync(
                userId);

        Assert.NotNull(account);
        Assert.Equal(
            userId,
            account.Id);
        Assert.Equal(
            email,
            account.Email);
    }

    [Fact]
    public async Task GetAccountAsync_MissingUser_ReturnsNull()
    {
        await using var context =
            await AccountTestContext.CreateAsync();

        var account =
            await context.AccountService.GetAccountAsync(
                Guid.NewGuid());

        Assert.Null(account);
    }

    [Fact]
    public async Task DeleteAccountAsync_ExistingUser_DeletesUser()
    {
        await using var context =
            await AccountTestContext.CreateAsync();

        const string email = "user@example.com";

        var userId = await context.CreateUserAsync(
            email);

        var result =
            await context.AccountService.DeleteAccountAsync(
                userId);

        var user =
            await context.UserManager.FindByIdAsync(
                userId.ToString());

        Assert.True(result.Succeeded);
        Assert.Empty(result.Errors);
        Assert.Null(user);
    }

    [Fact]
    public async Task DeleteAccountAsync_MissingUser_ReturnsFailure()
    {
        await using var context =
            await AccountTestContext.CreateAsync();

        var result =
            await context.AccountService.DeleteAccountAsync(
                Guid.NewGuid());

        Assert.False(result.Succeeded);

        var error =
            Assert.Single(result.Errors);

        Assert.Equal(
            "UserNotFound",
            error.Code);
        Assert.Equal(
            "User was not found.",
            error.Description);
    }

    private sealed class AccountTestContext
        : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ServiceProvider _serviceProvider;
        private readonly AsyncServiceScope _scope;

        private AccountTestContext(
            SqliteConnection connection,
            ServiceProvider serviceProvider,
            AsyncServiceScope scope,
            KabayanDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            _connection = connection;
            _serviceProvider = serviceProvider;
            _scope = scope;

            UserManager = userManager;

            AccountService = new AccountService(
                dbContext,
                userManager);
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public AccountService AccountService { get; }

        public async Task<Guid> CreateUserAsync(
            string email)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email
            };

            var result = await UserManager.CreateAsync(
                user,
                ValidPassword);

            Assert.True(result.Succeeded);

            return user.Id;
        }

        public static async Task<AccountTestContext> CreateAsync()
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

            return new AccountTestContext(
                connection,
                serviceProvider,
                scope,
                dbContext,
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
