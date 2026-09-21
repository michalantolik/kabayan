using Kabayan.Infrastructure.Common.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Kabayan.Infrastructure.Persistence;

/// <summary>
/// Provides database access for Launchpad infrastructure.
/// </summary>
internal sealed class KabayanDbContext
    : IdentityDbContext<
        ApplicationUser,
        IdentityRole<Guid>,
        Guid>
{
    public KabayanDbContext(
        DbContextOptions<KabayanDbContext> options)
        : base(options)
    {
    }
}
