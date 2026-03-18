using System.ComponentModel.DataAnnotations;

namespace AI_Workshop_App.DTOs;

/// <summary>
/// Request payload to set or update a person's headshot URL.
/// </summary>
public sealed class HeadshotRequest
{
    /// <summary>
    /// The URL pointing to the person's headshot image.
    /// </summary>
    [Required]
    [StringLength(2000)]
    public string HeadshotUrl { get; set; } = string.Empty;
}
