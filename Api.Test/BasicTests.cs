using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Api.Test;

public class BasicTests(WebApplicationFactory<Program> factory)
            : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> factory = factory;

    [Fact]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/weatherforecast/1/hallo/1");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }

    [Fact]
    public async Task Get_EndpointsReturnCustomerErrorDtoOnRoutingFailure()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/weatherforecast/1/hallo/error");

        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());
    }
}
