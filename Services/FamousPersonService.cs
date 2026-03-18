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
        CancellationToken cancellationToken = default)
    {
        var person = new FamousPerson
        {
            Id = Guid.NewGuid(),
            FullName = fullName.Trim(),
            Background = background.Trim(),
            CompetenceField = competenceField.Trim()
        };

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

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
