using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using FinanceTracker.Components.Data;

namespace FinanceTracker.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
     private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
    private User _currentUser;
    public void SetUser(User user)
    {
        _currentUser = user;
        var identity = user != null ? new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.Login)
        }, "auth") : _anonymous.Identity;
        var principal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = _currentUser != null ? new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, _currentUser.Login)
        }, "auth") : _anonymous.Identity;
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }
    public void Logout()
    {
        _currentUser = null;
        var principal = new ClaimsPrincipal(_anonymous.Identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

}
