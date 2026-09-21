using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Kabayan.Web.Features.Authentication.Pages;

public partial class Me
{
    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask
    {
        get;
        set;
    } = default!;

    private CurrentUserResponse? _currentUser;
    private bool _loading = true;
    private bool _error;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var authenticationState =
                await AuthenticationStateTask;

            _currentUser = await ApiClient.GetCurrentUserAsync(
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
}
