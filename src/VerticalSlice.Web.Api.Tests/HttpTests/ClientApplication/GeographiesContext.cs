using System.Net.Http.Json;
using System.Text.Json;
using VerticalSlice.Web.Api.Contracts.Response;

namespace VerticalSlice.Web.Api.Tests.HttpTests.ClientApplication;

public class GeographiesContext(HttpClient httpClient) : ContextBase(httpClient)
{
    public Task The_response_should_contain_geography_data()
    {
        Assert.That(Response, Is.Not.Null);
        Assert.That(Response!.Content.Headers.ContentType?.MediaType, Is.EqualTo("application/json"));
        return Task.CompletedTask;
    }

    public async Task The_response_should_contain_paged_geography_results()
    {
        Assert.That(Response, Is.Not.Null);
        ResponseContent = await Response!.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PagedGeographyResponse>(
            ResponseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Data, Is.Not.Null);
        Assert.That(result.Pagination, Is.Not.Null);
        Assert.That(result.Pagination.TotalItems, Is.GreaterThan(0));
    }

    public async Task The_response_should_contain_a_single_geography()
    {
        Assert.That(Response, Is.Not.Null);
        ResponseContent = await Response!.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GeographySummary>(
            ResponseContent,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.GeographyId, Is.GreaterThan(0));
        Assert.That(result.Name, Is.Not.Null.And.Not.Empty);
        Assert.That(result.ShortCode, Is.Not.Null.And.Not.Empty);
    }
}
