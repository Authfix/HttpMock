using System.Net;
using System.Net.Http.Json;
using Authfix.HttpMock;
using FluentAssertions;
using HttpMock.Tests.Dto;

namespace HttpMock.Tests;

[TestClass]
public class HttpMockTests
{
    [TestMethod]
    public async Task Should_ReturnTypedObjects_When_FileExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttpMessageHandler);

        // Act
        var response = await httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("http://localhost/api/users");

        // Assert
        response!.Count().Should().Be(2);
    }

    [TestMethod]
    public async Task Should_ReturnOkCode_When_FileExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttpMessageHandler);

        // Act
        var response = await httpClient.GetAsync("http://localhost/api/users");

        // Assert
        response!.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [TestMethod]
    public async Task Should_ReturnNotFoundCode_When_FileNotExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttpMessageHandler);

        // Act
        var response = await httpClient.GetAsync("http://localhost/api/not-found");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [TestMethod]
    public async Task Should_ReturnCreatedCode_When_FileExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        var httpClient = new HttpClient(mockHttpMessageHandler);

        // Act
        var response = await httpClient.PostAsync("http://localhost/api/users", new StringContent(@""" { id: 1 } """));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [TestMethod]
    public async Task Should_CallScenarioSuccessfully_When_FileExists()
    {
        // Arrange
        var mockHttpMessageHandler = new MockHttpMessageHandler(100);
        var httpClient = new HttpClient(mockHttpMessageHandler);

        // Act
        var firstResponse = await httpClient.GetAsync("http://localhost/api/scenari/2");
        var secondResponse = await httpClient.DeleteAsync("http://localhost/api/scenari/2");

        // Assert
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}