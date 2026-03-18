using AI_Workshop_App.Models;
using Microsoft.EntityFrameworkCore;

namespace AI_Workshop_App.Data;

/// <summary>
/// EF Core database context for the application.
/// </summary>
public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    /// <summary>
    /// The famous people currently stored by the app.
    /// </summary>
    public DbSet<FamousPerson> FamousPeople => Set<FamousPerson>();
}
