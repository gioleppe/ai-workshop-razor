using AI_Workshop_App.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_Workshop_App.Data;

/// <summary>
/// Seeds the in-memory database with demo data.
/// </summary>
public static class AppDbInitializer
{
    /// <summary>
    /// Ensures the in-memory store exists and contains starter data.
    /// </summary>
    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.FamousPeople.AnyAsync())
        {
            return;
        }

        dbContext.FamousPeople.AddRange(
            new FamousPerson
            {
                Id = Guid.NewGuid(),
                FullName = "Ada Lovelace",
                Background = "English mathematician and writer who is widely regarded as the first computer programmer because of her work on Charles Babbage's Analytical Engine.",
                CompetenceField = "Computer Science"
            },
            new FamousPerson
            {
                Id = Guid.NewGuid(),
                FullName = "Marie Curie",
                Background = "Physicist and chemist whose pioneering research on radioactivity made her the first woman to win a Nobel Prize and the first person to win it twice.",
                CompetenceField = "Physics"
            },
            new FamousPerson
            {
                Id = Guid.NewGuid(),
                FullName = "Nelson Mandela",
                Background = "South African anti-apartheid leader and president celebrated for his lifelong work advancing reconciliation, justice, and democracy.",
                CompetenceField = "Politics"
            });

        await dbContext.SaveChangesAsync();
    }
}
