using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace VerticalSlice.Web.Api.Data;

/// <summary>
///     Seeds the database using configurable seed data providers.
///     Supports multiple data sets (Intelligence, Ecommerce, Financial Institution).
///     Default is Intelligence Community data set.
/// </summary>
[ExcludeFromCodeCoverage]
public static class DatabaseSeeder
{

    /// <summary>
    ///     Seeds the database using the provided seed data provider.
    /// </summary>
    public static async Task SeedAsync(
        VerticalSliceDataContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Seed data in order of dependencies
        await SeedGeographiesAsync(context);
        await SeedAuditsAsync(context);

        await context.SaveChangesAsync();
    }

    private static async Task SeedGeographiesAsync(VerticalSliceDataContext context)
    {
        if (await context.Geographies.AnyAsync())
        {
            return;
        }

        var geographies = new List<Model.Geography>
        {
            new() { Name = "United States", ShortCode = "US", Region = "Americas", SubRegion = "Northern America", Capital = "Washington, D.C.", Population = 331000000, AreaKm2 = 9833517, GeoCodes = "{\"latitude\":37.09,\"longitude\":-95.71}" },
            new() { Name = "United Kingdom", ShortCode = "GB", Region = "Europe", SubRegion = "Northern Europe", Capital = "London", Population = 67000000, AreaKm2 = 243610, GeoCodes = "{\"latitude\":55.38,\"longitude\":-3.44}" },
            new() { Name = "Germany", ShortCode = "DE", Region = "Europe", SubRegion = "Western Europe", Capital = "Berlin", Population = 83000000, AreaKm2 = 357114, GeoCodes = "{\"latitude\":51.17,\"longitude\":10.45}" },
            new() { Name = "France", ShortCode = "FR", Region = "Europe", SubRegion = "Western Europe", Capital = "Paris", Population = 67000000, AreaKm2 = 551695, GeoCodes = "{\"latitude\":46.23,\"longitude\":2.21}" },
            new() { Name = "Japan", ShortCode = "JP", Region = "Asia", SubRegion = "Eastern Asia", Capital = "Tokyo", Population = 126000000, AreaKm2 = 377930, GeoCodes = "{\"latitude\":36.20,\"longitude\":138.25}" },
            new() { Name = "China", ShortCode = "CN", Region = "Asia", SubRegion = "Eastern Asia", Capital = "Beijing", Population = 1400000000, AreaKm2 = 9596960, GeoCodes = "{\"latitude\":35.86,\"longitude\":104.20}" },
            new() { Name = "India", ShortCode = "IN", Region = "Asia", SubRegion = "Southern Asia", Capital = "New Delhi", Population = 1380000000, AreaKm2 = 3287263, GeoCodes = "{\"latitude\":20.59,\"longitude\":78.96}" },
            new() { Name = "Australia", ShortCode = "AU", Region = "Oceania", SubRegion = "Australia and New Zealand", Capital = "Canberra", Population = 25000000, AreaKm2 = 7692024, GeoCodes = "{\"latitude\":-25.27,\"longitude\":133.78}" },
            new() { Name = "Brazil", ShortCode = "BR", Region = "Americas", SubRegion = "South America", Capital = "Brasília", Population = 213000000, AreaKm2 = 8515767, GeoCodes = "{\"latitude\":-14.24,\"longitude\":-51.93}" },
            new() { Name = "Canada", ShortCode = "CA", Region = "Americas", SubRegion = "Northern America", Capital = "Ottawa", Population = 38000000, AreaKm2 = 9984670, GeoCodes = "{\"latitude\":56.13,\"longitude\":-106.35}" },
            new() { Name = "South Africa", ShortCode = "ZA", Region = "Africa", SubRegion = "Sub-Saharan Africa", Capital = "Pretoria", Population = 60000000, AreaKm2 = 1221037, GeoCodes = "{\"latitude\":-30.56,\"longitude\":22.94}" },
            new() { Name = "Nigeria", ShortCode = "NG", Region = "Africa", SubRegion = "Sub-Saharan Africa", Capital = "Abuja", Population = 206000000, AreaKm2 = 923768, GeoCodes = "{\"latitude\":9.08,\"longitude\":8.68}" },
            new() { Name = "Mexico", ShortCode = "MX", Region = "Americas", SubRegion = "Central America", Capital = "Mexico City", Population = 128000000, AreaKm2 = 1964375, GeoCodes = "{\"latitude\":23.63,\"longitude\":-102.55}" },
            new() { Name = "Italy", ShortCode = "IT", Region = "Europe", SubRegion = "Southern Europe", Capital = "Rome", Population = 60000000, AreaKm2 = 301340, GeoCodes = "{\"latitude\":41.87,\"longitude\":12.57}" },
            new() { Name = "Spain", ShortCode = "ES", Region = "Europe", SubRegion = "Southern Europe", Capital = "Madrid", Population = 47000000, AreaKm2 = 505990, GeoCodes = "{\"latitude\":40.46,\"longitude\":-3.75}" },
        };

        await context.Geographies.AddRangeAsync(geographies);
    }

    private static async Task SeedAuditsAsync(VerticalSliceDataContext context)
    {
        if (await context.Audit.AnyAsync())
        {
            return;
        }

        var rand = new Random();
        var now = DateTime.UtcNow;

        string[] operations = { "CREATE", "UPDATE", "DELETE", "READ", "LOGIN", "LOGOUT", "IMPORT", "EXPORT", "PATCH" };
        string[] entityTypes = { "Node", "Organization", "User", "RiskLink", "NodeType", "Geography", "Alert", "Report" };
        (string id, string name)[] users =
        {
            ("u-1", "alice@example.com"),
            ("u-2", "bob@example.com"),
            ("u-3", "carol@example.com"),
            ("u-4", "dave@example.com"),
            ("u-5", "eve@example.com")
        };
        string[] methods = { "GET", "POST", "PUT", "PATCH", "DELETE" };
        string[] endpoints = { "/api/v1/nodes", "/api/v1/organizations", "/api/v1/audit", "/api/v1/node-types", "/api/v1/risk-links" };
        string[] userAgents = { "Mozilla/5.0", "curl/7.79.1", "PostmanRuntime/7.29.0", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)" };

        var audits = new List<Model.Audit>(capacity: 500);

        for (int i = 0; i < 500; i++)
        {
            string op = operations[rand.Next(operations.Length)];
            string entity = entityTypes[rand.Next(entityTypes.Length)];
            string entityId = (rand.Next(1, 2000)).ToString();
            var user = users[rand.Next(users.Length)];

            DateTime timestamp = now - TimeSpan.FromDays(rand.Next(0, 90)) - TimeSpan.FromSeconds(rand.Next(0, 86400));

            string? oldValues = null;
            string? newValues = null;
            if (op == "UPDATE")
            {
                oldValues = $"{{ \"name\": \"{entity}-old-{rand.Next(1,1000)}\" }}";
                newValues = $"{{ \"name\": \"{entity}-new-{rand.Next(1,1000)}\" }}";
            }
            else if (op == "CREATE")
            {
                newValues = $"{{ \"name\": \"{entity}-created-{rand.Next(1,10000)}\" }}";
            }

            var audit = new Model.Audit
            {
                Operation = op,
                EntityType = entity,
                EntityId = entityId,
                UserId = user.id,
                UserName = user.name,
                Timestamp = timestamp,
                IpAddress = $"{rand.Next(1,255)}.{rand.Next(0,255)}.{rand.Next(0,255)}.{rand.Next(0,255)}",
                UserAgent = userAgents[rand.Next(userAgents.Length)],
                OldValues = oldValues,
                NewValues = newValues,
                Context = rand.NextDouble() > 0.9 ? "batch-import" : null,
                HttpMethod = methods[rand.Next(methods.Length)],
                Endpoint = endpoints[rand.Next(endpoints.Length)],
                IsSuccess = rand.NextDouble() > 0.05,
                ErrorMessage = null,
                OrganizationId = rand.NextDouble() > 0.7 ? $"org-{rand.Next(1,20)}" : null,
                Tags = rand.NextDouble() > 0.85 ? "automated,import" : null,
                DurationMs = rand.Next(5, 5000),
                CorrelationId = Guid.NewGuid().ToString()
            };

            if (!audit.IsSuccess)
            {
                audit.ErrorMessage = "Simulated failure: unexpected error";
            }

            audits.Add(audit);
        }

        await context.Audit.AddRangeAsync(audits);
    }
}
