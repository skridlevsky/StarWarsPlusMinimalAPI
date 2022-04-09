
# Star Wars+ API

Personal assignment to demonstrate web API development knowledge. This API contains endpoint that relates to residents of the vast Star Wars galaxy.

It is built using ASP.NET Core 6 that uses the new minimal API feature - instead of going the traditional MVC ceremony, I'm delving into exploring this recently introduced approach.

Technologies used:
* [Minimal APIs](https://devblogs.microsoft.com/aspnet/asp-net-core-updates-in-net-6-preview-4/#introducing-minimal-apis) for exploration.
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (code-first approach) paired with [SQLite](https://docs.microsoft.com/en-us/ef/core/providers/sqlite/) for persistence.
* [Swagger](https://swagger.io/docs/) for API documentation.
* [Docker](https://docs.docker.com/) for containerization.
* [XUnit](https://xunit.net/) for testing.

## Running and Testing

In order to run the application:
```bash
$ cd StarWarsPlusMinimalAPI
$ dotnet watch run # Runs and opens your browser to Swagger page
```

To see if all tests are passing:
```bash
$ cd StarWarsPlusMinimalAPI.Tests
$ dotnet test
```

# Future Improvements - Beyond the One Hour
- [ ] Restructure and decouple existing structure into [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/) - here is an [example](https://github.com/isaacOjeda/MinimalApiArchitecture) for reference.
- [ ] Add validations to POST/PUT.
- [ ] Store Homeworlds and Species in the database.
- [ ] Use some form of authorization for the API, probably not a good idea to leave access out in the open to modifying/deleting everything.
- [ ] Create GitHub Action (or other) to perform CI and possibly CD.
- [ ] Exploring and learning even more about minimal APIs to fully understand when it's best to use and when not to.
- [ ] [Always being one with the Force](https://www.youtube.com/watch?v=M9iYT3lhmd4).
