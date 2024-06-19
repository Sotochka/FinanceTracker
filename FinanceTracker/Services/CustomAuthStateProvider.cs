using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using FinanceTracker.Components.Data;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FinanceTracker.Services;
public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ProtectedSessionStorage _sessionStorage;
    private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthStateProvider(ProtectedSessionStorage sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var userSessionStorageResult = await _sessionStorage.GetAsync<User>("CurrentUser");
            var user = userSessionStorageResult.Success ? userSessionStorageResult.Value : null;

            if (user != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim("UserId", user.Id.ToString())
                }, "auth");

                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch
        {
            // Handle error retrieving user from session storage
        }

        return new AuthenticationState(_anonymous);
    }

    public async Task SetUser(User user)
    {
        ClaimsIdentity identity;

        if (user != null)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim("UserId", user.Id.ToString())
            }, "auth");

            await _sessionStorage.SetAsync("CurrentUser", user);
        }
        else
        {
            identity = (ClaimsIdentity)_anonymous.Identity;
            await _sessionStorage.DeleteAsync("CurrentUser");
        }

        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
    public async Task LogoutAsync()
    {
        await _sessionStorage.DeleteAsync("CurrentUser");
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
    }
}
