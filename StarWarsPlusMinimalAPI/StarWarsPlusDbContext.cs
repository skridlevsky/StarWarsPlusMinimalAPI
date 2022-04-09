using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class StarWarsPlusDbContext: DbContext
{
    public StarWarsPlusDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Resident> Residents { get; set; }
}

public class Resident
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int Height { get; set; }
    public string Homeworld { get; set; }
    public Gender Gender { get; set; }
    public string Specie { get; set; }
    public DateTime Created { get; set; }
    public DateTime Edited { get; set; }

    public Resident()
    {
        this.Created = DateTime.UtcNow;
        this.Edited = DateTime.UtcNow;
    }

    // TODO: Create separate tables that contain all galaxy homeworlds & species with 1:1 relation to residents
    //public Homeworld Homeworld { get; set; }
    //public Specie Specie { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    Male,
    Female,
    NonBinary,
    PreferNotToSay
}