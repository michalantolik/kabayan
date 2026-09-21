using Microsoft.AspNetCore.Identity;

namespace Kabayan.Infrastructure.Common.Identity;

/// <summary>
/// Represents an application user.
/// </summary>
internal sealed class ApplicationUser
    : IdentityUser<Guid>
{
}
