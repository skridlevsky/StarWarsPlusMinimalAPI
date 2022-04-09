using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Xunit;

public class ResidentTests
{
    [Fact]
    public async Task GetResidents()
    {
        await using var application = new StarWarsPlusApplication();

        var client = application.CreateClient();
        var residents = await client.GetFromJsonAsync<List<Resident>>("/residents");

        Assert.Empty(residents);
    }

    [Fact]
    public async Task GetConnectionsForHomeworld()
    {
        await using var application = new StarWarsPlusApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync(
            "/residents",
            new Resident
            {
                Name = "Luke Skywalker",
                Height = 177,
                Gender = Gender.Male,
                Homeworld = "Tatooine",
                Specie = "Human"
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var residents = await client.GetFromJsonAsync<List<Resident>>("/residents");

        var resident = Assert.Single(residents);
        Assert.Equal("Luke Skywalker", resident.Name);

        var connections = await client.GetFromJsonAsync<Connections>($"/connections?homeworld=Tatooine");
        Assert.Single(connections.Results);

        connections = await client.GetFromJsonAsync<Connections>($"/connections?homeworld=Coruscant");
        Assert.Empty(connections.Results);
    }

    [Fact]
    public async Task PostResident()
    {
        await using var application = new StarWarsPlusApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync(
            "/residents",
            new Resident
            {
                Name = "Luke Skywalker",
                Height = 177,
                Gender = Gender.Male,
                Homeworld = "Tatooine",
                Specie = "Human"
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var residents = await client.GetFromJsonAsync<List<Resident>>("/residents");

        var resident = Assert.Single(residents);
        Assert.Equal("Luke Skywalker", resident.Name);
    }

    [Fact]
    public async Task PutResident()
    {
        await using var application = new StarWarsPlusApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync(
            "/residents",
            new Resident
            {
                Name = "Luke Skywalker",
                Height = 177,
                Gender = Gender.Male,
                Homeworld = "Tatooine",
                Specie = "Human"
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var residents = await client.GetFromJsonAsync<List<Resident>>("/residents");

        var resident = Assert.Single(residents);
        Assert.Equal("Luke Skywalker", resident.Name);

        response = await client.PutAsJsonAsync($"/residents/{resident.Id}", new Resident { Name = "Wormie" });
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        response = await client.PutAsJsonAsync($"/residents/{resident.Id + 1}", new Resident { Name = "Wormie" });
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteResidents()
    {
        await using var application = new StarWarsPlusApplication();

        var client = application.CreateClient();
        var response = await client.PostAsJsonAsync(
            "/residents",
            new Resident
            {
                Name = "Luke Skywalker",
                Height = 177,
                Gender = Gender.Male,
                Homeworld = "Tatooine",
                Specie = "Human"
            });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var residents = await client.GetFromJsonAsync<List<Resident>>("/residents");

        var resident = Assert.Single(residents);
        Assert.Equal("Luke Skywalker", resident.Name);

        response = await client.DeleteAsync($"/residents/{resident.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        response = await client.GetAsync($"/residents/{resident.Id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}

class StarWarsPlusApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<StarWarsPlusDbContext>));

            services.AddDbContext<StarWarsPlusDbContext>(options =>
                options.UseInMemoryDatabase("Testing", root));
        });

        return base.CreateHost(builder);
    }
}