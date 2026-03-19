using AI_Workshop_App.Models;

namespace AI_Workshop_App.Services;

/// <summary>
/// Shared application service for managing famous people.
/// </summary>
public interface IFamousPersonService
{
    /// <summary>
    /// Retrieves all famous people currently stored in the app.
    /// </summary>
    Task<IReadOnlyList<FamousPerson>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new famous person to the app.
    /// </summary>
    Task<FamousPerson> AddAsync(
        string fullName,
        string background,
        string competenceField,
        string? headshotUrl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing famous person if it exists.
    /// </summary>
    Task<bool> UpdateAsync(
        Guid id,
        string fullName,
        string background,
        string competenceField,
        string? headshotUrl = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates only the headshot URL for the specified famous person.
    /// </summary>
    Task<bool> UpdateHeadshotAsync(
        Guid id,
        string? headshotUrl,
        CancellationToken cancellationToken = default);
}
