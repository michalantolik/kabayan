using Kabayan.Application.Account;
using Kabayan.Application.Account.DeleteAccount;
using Moq;

namespace Kabayan.Application.Tests.Account.DeleteAccount;

public sealed class DeleteAccountHandlerTests
{
    [Fact]
    public async Task HandleAsync_ExistingAccount_ReturnsDeleteResult()
    {
        var userId = Guid.NewGuid();

        var deleteResult =
            DeleteAccountResult.Success();

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var accountService =
            new Mock<IAccountService>();

        accountService
            .Setup(service => service.DeleteAccountAsync(
                userId,
                cancellationTokenSource.Token))
            .ReturnsAsync(deleteResult);

        var handler = new DeleteAccountHandler(
            accountService.Object);

        var command = new DeleteAccountCommand(
            userId);

        var result = await handler.HandleAsync(
            command,
            cancellationTokenSource.Token);

        Assert.Equal(
            deleteResult,
            result);

        accountService.Verify(
            service => service.DeleteAccountAsync(
                userId,
                cancellationTokenSource.Token),
            Times.Once);
    }
}
