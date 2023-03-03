using System.Net;
using Authfix.HttpMock;
using FluentAssertions;

namespace HttpMock.Tests;

[TestClass]
public class HttpMockTests
{
    [TestMethod]
    public async Task Should_GetResponse_When_FileExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttpMessageHandler);
        
        // Act
        var response = await httpClient.GetAsync("http://localhost/api/users");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task Should_ReturnNotFound_When_FileNotExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttpMessageHandler);
        
        // Act
        var response = await httpClient.GetAsync("http://localhost/api/not-found");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}