using Microsoft.AspNetCore.Components;

namespace Kabayan.Web.Features.Authentication.Pages;

public partial class Register
{
    [SupplyParameterFromQuery(Name = "errors")]
    private string? Errors { get; set; }

    [SupplyParameterFromQuery(Name = "status")]
    private string? Status { get; set; }

    private bool IsCreated =>
        string.Equals(
            Status,
            "created",
            StringComparison.Ordinal);

    private IReadOnlyList<string> ErrorMessages
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Errors))
            {
                return [];
            }

            return Errors
                .Split(
                    '\n',
                    StringSplitOptions.RemoveEmptyEntries |
                    StringSplitOptions.TrimEntries)
                .Distinct(StringComparer.Ordinal)
                .ToArray();
        }
    }
}
