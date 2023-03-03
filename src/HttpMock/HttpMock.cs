using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using Authfix.HttpMock.Extensions;
using Authfix.HttpMock.Providers;

namespace Authfix.HttpMock;

public class MockHttpMessageHandler : HttpMessageHandler
{
    private const string DefaultFolder = "Responses";
    private readonly IContentProvider _contentProvider;

    public MockHttpMessageHandler(): this(new ResourceContentProvider(Assembly.GetCallingAssembly(), DefaultFolder))
    {
    }
    
    private MockHttpMessageHandler(IContentProvider contentProvider)
    {
        _contentProvider = contentProvider ?? throw new ArgumentNullException(nameof(contentProvider));
    }
    
    /// <inheritdoc cref="SendAsync"/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Debug.Assert(request.RequestUri != null, "request.RequestUri cannot be null");
        
        try
        {           
            var responseFileName = $"{request.Method.ToString().ToUpper()}_{request.Method.ToDefaultResultCode()}{request.RequestUri.AbsolutePath.Replace("/", "_").ToUpper()}.json";
            var content = await _contentProvider.ReadAsync(responseFileName);
            
            var response = new HttpResponseMessage((HttpStatusCode)request.Method.ToDefaultResultCode());
            response.Content = new StreamContent(content);

            return response;
        }
        catch (ContentNotFoundException)
        {
            var notFoundResponseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            return notFoundResponseMessage;
        }
    }
}