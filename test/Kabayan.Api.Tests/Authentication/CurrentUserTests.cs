using Kabayan.Api.Common.Identity;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Kabayan.Api.Tests.Authentication;

public sealed class CurrentUserTests
{
    [Fact]
    public void UserId_WithValidSubject_ReturnsUserId()
    {
        var expectedUserId = Guid.NewGuid();
        var currentUser = CreateCurrentUser(
            new Claim(
                JwtRegisteredClaimNames.Sub,
                expectedUserId.ToString()));

        Assert.Equal(
            expectedUserId,
            currentUser.UserId);
    }

    [Fact]
    public void UserId_WithoutSubject_ThrowsInvalidOperationException()
    {
        var currentUser = CreateCurrentUser();

        Assert.Throws<InvalidOperationException>(
            () => currentUser.UserId);
    }

    [Fact]
    public void UserId_WithInvalidSubject_ThrowsInvalidOperationException()
    {
        var currentUser = CreateCurrentUser(
            new Claim(
                JwtRegisteredClaimNames.Sub,
                "not-a-guid"));

        Assert.Throws<InvalidOperationException>(
            () => currentUser.UserId);
    }

    private static CurrentUser CreateCurrentUser(
        params Claim[] claims)
    {
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity(claims))
        };

        return new CurrentUser(
            new HttpContextAccessor
            {
                HttpContext = httpContext
            });
    }
}
