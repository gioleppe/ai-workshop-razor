namespace AI_Workshop_App.Models;

/// <summary>
/// Represents a well-known person stored in the application.
/// </summary>
public sealed class FamousPerson
{
    /// <summary>
    /// Unique identifier for the famous person.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Full first and last name of the person.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Short biography or historical background for the person.
    /// </summary>
    public required string Background { get; set; }

    /// <summary>
    /// Field in which the person is especially known.
    /// </summary>
    public required string CompetenceField { get; set; }

    /// <summary>
    /// Optional URL to a headshot image for the person.
    /// </summary>
    public string? HeadshotUrl { get; set; }
}
