using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kabayan.Web.Features.Account.Pages;

public partial class Account
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask
    {
        get;
        set;
    } = default!;

    private AccountDetails? _account;
    private bool _loading = true;
    private bool _error;
    private bool _confirmingDeletion;
    private bool _deleteConfirmed;
    private bool _deleting;
    private bool _deleteFailed;
    private bool _accountDeleted;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState =
                await AuthenticationStateTask;

            _account = await ApiClient.GetAccountAsync(
                authenticationState.User);
        }
        catch
        {
            _error = true;
        }
        finally
        {
            _loading = false;
        }
    }

    private void OpenDeleteConfirmation()
    {
        _confirmingDeletion = true;
        _deleteConfirmed = false;
        _deleteFailed = false;
    }

    private void CancelDeleteConfirmation()
    {
        _confirmingDeletion = false;
        _deleteConfirmed = false;
        _deleteFailed = false;
    }

    private async Task DeleteAccountAsync()
    {
        if (!_deleteConfirmed)
        {
            return;
        }

        _deleteFailed = false;
        _deleting = true;

        try
        {
            var authenticationState =
                await AuthenticationStateTask;

            var succeeded =
                await ApiClient.DeleteAccountAsync(
                    authenticationState.User);

            if (!succeeded)
            {
                _deleteFailed = true;
                return;
            }

            _accountDeleted = true;
        }
        catch
        {
            _deleteFailed = true;
        }
        finally
        {
            _deleting = false;
        }
    }
}
