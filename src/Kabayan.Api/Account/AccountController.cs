using Kabayan.Application.Account;
using Kabayan.Application.Account.DeleteAccount;
using Kabayan.Application.Account.GetAccount;
using Kabayan.Application.Common.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kabayan.Api.Account;

/// <summary>
/// Provides account management endpoints.
/// </summary>
[ApiController]
[Authorize]
[Route("api/account")]
public sealed class AccountController(
    GetAccountHandler getAccountHandler,
    DeleteAccountHandler deleteAccountHandler,
    ICurrentUser currentUser)
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<AccountDto>> GetAsync(
        CancellationToken cancellationToken)
    {
        var account = await getAccountHandler.HandleAsync(
            currentUser.UserId,
            cancellationToken);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(
        CancellationToken cancellationToken)
    {
        var command = new DeleteAccountCommand(
            currentUser.UserId);

        var result = await deleteAccountHandler.HandleAsync(
            command,
            cancellationToken);

        if (!result.Succeeded)
        {
            return Problem(
                detail: string.Join(
                    Environment.NewLine,
                    result.Errors.Select(error =>
                        error.Description)),
                statusCode:
                    StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }
}
