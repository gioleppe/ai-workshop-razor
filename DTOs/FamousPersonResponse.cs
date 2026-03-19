namespace AI_Workshop_App.DTOs;

/// <summary>
/// Response payload representing a famous person stored in the app.
/// </summary>
public sealed class FamousPersonResponse
{
    /// <summary>
    /// Unique identifier for the famous person.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Full first and last name of the person.
    /// </summary>
    public string FullName { get; init; } = string.Empty;

    /// <summary>
    /// Brief biography or historical context for the person.
    /// </summary>
    public string Background { get; init; } = string.Empty;

    /// <summary>
    /// Domain or discipline in which the person is famous.
    /// </summary>
    public string CompetenceField { get; init; } = string.Empty;

    /// <summary>
    /// Optional URL to a headshot image for the person.
    /// </summary>
    public string? HeadshotUrl { get; init; }
}
