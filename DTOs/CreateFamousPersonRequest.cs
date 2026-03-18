using System.ComponentModel.DataAnnotations;

namespace AI_Workshop_App.DTOs;

/// <summary>
/// Request payload used to add a new famous person to the app.
/// </summary>
public sealed class CreateFamousPersonRequest
{
    /// <summary>
    /// Full first and last name of the person to add.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Brief biography or historical context for the person.
    /// </summary>
    [Required]
    [StringLength(4000)]
    public string Background { get; set; } = string.Empty;

    /// <summary>
    /// Domain or discipline in which the person is famous.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string CompetenceField { get; set; } = string.Empty;
}
