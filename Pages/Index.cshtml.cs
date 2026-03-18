using AI_Workshop_App.Models;
using AI_Workshop_App.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AI_Workshop_App.Pages;

/// <summary>
/// Root page displaying the current list of famous people.
/// </summary>
public sealed class IndexModel(IFamousPersonService famousPersonService) : PageModel
{
    /// <summary>
    /// The people currently available to show on the page.
    /// </summary>
    public IReadOnlyList<FamousPerson> People { get; private set; } = Array.Empty<FamousPerson>();

    /// <summary>
    /// Loads the current famous people from the shared service.
    /// </summary>
    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        People = await famousPersonService.GetAllAsync(cancellationToken);
    }
}
