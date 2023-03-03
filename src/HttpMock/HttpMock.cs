using System.Net;

namespace Authfix.HttpMock;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private const string DefaultFolder = ".\\Responses";
    
    private readonly string _baseFolder;

    public MockHttpMessageHandler(): this(DefaultFolder)
    {
    }
    
    public MockHttpMessageHandler(string baseFolder)
    {
        _baseFolder = baseFolder;
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var responseFileName = $"{request.Method.ToString().ToUpper()}_200{request.RequestUri.AbsolutePath.Replace("/", "_").ToUpper()}.json";
        var responseFilePath = Path.Combine(_baseFolder, responseFileName);
        
        if (!File.Exists(responseFilePath))
        {
            var notFoundResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            return notFoundResponseMessage;
        }

        var text = await File.ReadAllTextAsync(responseFilePath, cancellationToken);
        
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(text);

        return response;
    }
}