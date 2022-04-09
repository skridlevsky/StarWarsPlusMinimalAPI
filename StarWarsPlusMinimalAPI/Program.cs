using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<StarWarsPlusDbContext>(options => options.UseSqlite("Data Source=starwarsplus.db"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = builder.Environment.ApplicationName, Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapFallback(() => Results.Redirect("/swagger"));

app.MapGet("/residents", async (StarWarsPlusDbContext db) =>
    await db.Residents.ToListAsync())
    .WithName("GetAllGalaxyResidents");

app.MapGet("/residents/{id}", async (StarWarsPlusDbContext db, int id) =>
    await db.Residents.FindAsync(id)
        is Resident resident
            ? Results.Ok(resident)
            : Results.NotFound())
    .WithName("GetResidentById")
    .Produces<Resident>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status404NotFound);

app.MapGet("/connections", async (StarWarsPlusDbContext db, string homeworld) =>
{
        List<Resident> residents = await db.Residents.Where(r => r.Homeworld == homeworld).ToListAsync();

        // TODO: create a model for this response
        var connections = new Connections
        {
            Count = residents.Count,
            Results = residents.Select(r => r.Name).ToList()
        };

        return Results.Ok(connections);
})
        .WithName("GetConnectionsByHomeworld")
        .Produces<Connections>(StatusCodes.Status200OK); // TODO: provide produced model response


app.MapPost("/residents", async (StarWarsPlusDbContext db, Resident resident) =>
{
    // TODO: validate resident, return errors if validation failed

    db.Residents.Add(resident);
    await db.SaveChangesAsync();

    return Results.Created($"/todos/{resident.Id}", resident);
})
    .WithName("CreateResident")
    .ProducesValidationProblem()
    .Produces<Resident>(StatusCodes.Status201Created);

app.MapPut("/residents/{id}", async (StarWarsPlusDbContext db, Resident inputResident, int id) =>
{
    // TODO: validate resident, return errors if validation failed

    var resident = await db.Residents.FindAsync(id);

    if (resident is null) return Results.NotFound();

    resident.Name = inputResident.Name;
    resident.Height = inputResident.Height;
    resident.Homeworld = inputResident.Homeworld;
    resident.Gender = inputResident.Gender;
    resident.Specie = inputResident.Specie;

    await db.SaveChangesAsync();

    return Results.NoContent();
})
    .WithName("UpdateResident")
    .ProducesValidationProblem()
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent);

app.MapDelete("/residents/{id}", async (StarWarsPlusDbContext db, int id) =>
{
    var resident = await db.Residents.FindAsync(id);

    if (resident is null) return Results.NotFound();

    db.Residents.Remove(resident);

    await db.SaveChangesAsync();

    return Results.NoContent();
})
    .WithName("DeleteResident")
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status204NoContent);

app.MapDelete("/residents/delete-all", async (StarWarsPlusDbContext db) =>
    Results.Ok(await db.Database.ExecuteSqlRawAsync("DELETE FROM Residents")))
    .WithName("DeleteAllResidents")
    .Produces<int>(StatusCodes.Status200OK);

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }

public class Connections
{
    public int Count { get; set; }
    public List<string> Results { get; set; }
}