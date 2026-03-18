using AI_Workshop_App.DTOs;
using AI_Workshop_App.Models;
using AI_Workshop_App.Services;
using Microsoft.AspNetCore.Mvc;

namespace AI_Workshop_App.Controllers;

/// <summary>
/// Agent-facing endpoints for listing, creating, and updating famous people.
/// </summary>
[ApiController]
[Route("api/famouspeople")]
public sealed class FamousPeopleController(IFamousPersonService famousPersonService) : ControllerBase
{
    /// <summary>
    /// Retrieves the current list of famous people in the database.
    /// </summary>
    /// <remarks>
    /// Agents can call this endpoint before creating new data so they can inspect what is already available.
    /// </remarks>
    /// <response code="200">Returns the current list of famous people.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FamousPersonResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<FamousPersonResponse>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var people = await famousPersonService.GetAllAsync(cancellationToken);
        return Ok(people.Select(MapResponse));
    }

    /// <summary>
    /// Adds a new famous person to the database.
    /// </summary>
    /// <remarks>
    /// Agents use this endpoint after generating the data they want the app to store and display.
    /// </remarks>
    /// <response code="201">Returns the newly created famous person, including its generated identifier.</response>
    /// <response code="400">The request payload failed validation.</response>
    [HttpPost]
    [ProducesResponseType(typeof(FamousPersonResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FamousPersonResponse>> CreateAsync(
        [FromBody] CreateFamousPersonRequest request,
        CancellationToken cancellationToken)
    {
        var person = await famousPersonService.AddAsync(
            request.FullName,
            request.Background,
            request.CompetenceField,
            request.HeadshotUrl,
            cancellationToken);

        var response = MapResponse(person);
        return Created($"{Request.Path}/{person.Id}", response);
    }

    /// <summary>
    /// Updates an existing famous person in the database.
    /// </summary>
    /// <remarks>
    /// Agents use this endpoint to correct or enrich an existing person's details.
    /// </remarks>
    /// <param name="id">The identifier of the famous person to update.</param>
    /// <param name="request">The complete updated representation of the famous person.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <response code="204">The person was updated successfully.</response>
    /// <response code="400">The route identifier and payload identifier do not match, or the payload failed validation.</response>
    /// <response code="404">No famous person exists with the specified identifier.</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        [FromBody] UpdateFamousPersonRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Id != id)
        {
            ModelState.AddModelError(nameof(request.Id), "The route id must match the payload id.");
            return ValidationProblem(ModelState);
        }

        var updated = await famousPersonService.UpdateAsync(
            request.Id,
            request.FullName,
            request.Background,
            request.CompetenceField,
            request.HeadshotUrl,
            cancellationToken);

        return updated ? NoContent() : NotFound();
    }

    /// <summary>
    /// Updates only the headshot URL for a famous person.
    /// </summary>
    [HttpPut("{id:guid}/headshot")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateHeadshotAsync(
        Guid id,
        [FromBody] HeadshotRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null)
        {
            return BadRequest();
        }

        var updated = await famousPersonService.UpdateHeadshotAsync(id, request.HeadshotUrl, cancellationToken);
        return updated ? NoContent() : NotFound();
    }

    private static FamousPersonResponse MapResponse(FamousPerson person) =>
        new()
        {
            Id = person.Id,
            FullName = person.FullName,
            Background = person.Background,
            CompetenceField = person.CompetenceField,
            HeadshotUrl = person.HeadshotUrl
        };
}
