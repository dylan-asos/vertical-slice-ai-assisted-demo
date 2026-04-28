using System.Net;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using LightBDD.NUnit3;
using VerticalSlice.Web.Api.Tests.HttpTests.ClientApplication;

namespace VerticalSlice.Web.Api.Tests.HttpTests.Features;

[FeatureDescription(
    @"I want an API for accessing geography information
        So that I can view and manage geographical data in the system")]
public class GeographiesFeature : FeatureFixture
{
    [Scenario(Description = "Get list of geographies")]
    public async Task GetsGeographyList()
    {
        await Runner
            .WithContext<GeographiesContext>()
            .AddAsyncSteps(
                given => given.The_route_is_requested("api/v1/geographies"),
                then => then.The_response_code_should_be(HttpStatusCode.OK),
                and => and.The_response_should_contain_paged_geography_results()
            ).RunAsync();
    }

    [Scenario(Description = "Get a single geography by id")]
    public async Task GetsGeographyById()
    {
        await Runner
            .WithContext<GeographiesContext>()
            .AddAsyncSteps(
                given => given.The_route_is_requested("api/v1/geographies/1"),
                then => then.The_response_code_should_be(HttpStatusCode.OK),
                and => and.The_response_should_contain_a_single_geography()
            ).RunAsync();
    }

    [Scenario(Description = "Returns not found for unknown geography id")]
    public async Task ReturnsNotFoundForUnknownGeography()
    {
        await Runner
            .WithContext<GeographiesContext>()
            .AddAsyncSteps(
                given => given.The_route_is_requested("api/v1/geographies/999999"),
                then => then.The_response_code_should_be(HttpStatusCode.NotFound)
            ).RunAsync();
    }
}
