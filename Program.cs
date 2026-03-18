using AI_Workshop_App.Data;
using AI_Workshop_App.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRazorPages();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("FamousPeople"));
builder.Services.AddScoped<IFamousPersonService, FamousPersonService>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await AppDbInitializer.InitializeAsync(dbContext);
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapControllers();
app.MapRazorPages();

app.Run();
