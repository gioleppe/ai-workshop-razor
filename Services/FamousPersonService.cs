using AI_Workshop_App.Data;
using AI_Workshop_App.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_Workshop_App.Services;

/// <summary>
/// EF Core-backed implementation of <see cref="IFamousPersonService"/>.
/// </summary>
public sealed class FamousPersonService(AppDbContext dbContext) : IFamousPersonService
{
    /// <inheritdoc />
    public async Task<IReadOnlyList<FamousPerson>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.FamousPeople
            .AsNoTracking()
            .OrderBy(person => person.FullName)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<FamousPerson> AddAsync(
        string fullName,
        string background,
        string competenceField,
        string? headshotUrl = null,
        CancellationToken cancellationToken = default)
    {
        var person = new FamousPerson
        {
            Id = Guid.NewGuid(),
            FullName = fullName.Trim(),
            Background = background.Trim(),
            CompetenceField = competenceField.Trim()
        };

        // Assign a sensible default headshot URL when none provided.
        person.HeadshotUrl = string.IsNullOrWhiteSpace(headshotUrl)
            ? $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(person.FullName)}&size=256"
            : headshotUrl.Trim();

        dbContext.FamousPeople.Add(person);
        await dbContext.SaveChangesAsync(cancellationToken);

        return person;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(
        Guid id,
        string fullName,
        string background,
        string competenceField,
        string? headshotUrl = null,
        CancellationToken cancellationToken = default)
    {
        var person = await dbContext.FamousPeople.FirstOrDefaultAsync(
            existingPerson => existingPerson.Id == id,
            cancellationToken);

        if (person is null)
        {
            return false;
        }

        person.FullName = fullName.Trim();
        person.Background = background.Trim();
        person.CompetenceField = competenceField.Trim();

        if (headshotUrl is not null)
        {
            person.HeadshotUrl = string.IsNullOrWhiteSpace(headshotUrl) ? null : headshotUrl.Trim();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateHeadshotAsync(
        Guid id,
        string? headshotUrl,
        CancellationToken cancellationToken = default)
    {
        var person = await dbContext.FamousPeople.FirstOrDefaultAsync(
            existingPerson => existingPerson.Id == id,
            cancellationToken);

        if (person is null)
        {
            return false;
        }

        person.HeadshotUrl = string.IsNullOrWhiteSpace(headshotUrl) ? null : headshotUrl.Trim();
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
