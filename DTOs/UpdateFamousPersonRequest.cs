using System.ComponentModel.DataAnnotations;

namespace AI_Workshop_App.DTOs;

/// <summary>
/// Request payload used to update an existing famous person in the app.
/// </summary>
public sealed class UpdateFamousPersonRequest
{
    /// <summary>
    /// Unique identifier of the famous person to update.
    /// </summary>
    [Required]
    public Guid Id { get; set; }

    /// <summary>
    /// Full first and last name of the person after the update.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Updated biography or historical context for the person.
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string Background { get; set; } = string.Empty;

    /// <summary>
    /// Updated domain or discipline in which the person is famous.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string CompetenceField { get; set; } = string.Empty;

    /// <summary>
    /// Optional URL pointing to a headshot image for the person.
    /// </summary>
    [StringLength(2000)]
    public string? HeadshotUrl { get; set; }
}
