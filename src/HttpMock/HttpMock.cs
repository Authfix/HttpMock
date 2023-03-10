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
    private readonly int? _scenario;

    public MockHttpMessageHandler(int? scenario = null) : this(scenario,
        new ResourceContentProvider(Assembly.GetCallingAssembly(), DefaultFolder))
    {
    }

    private MockHttpMessageHandler(int? scenario, IContentProvider contentProvider)
    {
        _contentProvider = contentProvider ?? throw new ArgumentNullException(nameof(contentProvider));
        _scenario = scenario;
    }

    /// <inheritdoc cref="SendAsync"/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Debug.Assert(request.RequestUri != null, "request.RequestUri cannot be null");

        try
        {
            var responseFileNameBuilder = new StringBuilder();
            HttpStatusCode code = request.Method.ToDefaultResultCode();

            if (_scenario != null)
            {
                responseFileNameBuilder.Append($"_{_scenario}");
            }

            // We don't begin with an underscore as absolute path begin with a / (replaced by an underscore)
            responseFileNameBuilder.Append(request.RequestUri.AbsolutePath.Replace("/", "_").ToUpper());
            responseFileNameBuilder.Append($"_{request.Method.ToString().ToUpper()}");

            if (_scenario != null)
            {
                var availableResource = (await _contentProvider.GetListAsync()).ToList();
                var existingResource =
                    availableResource.FirstOrDefault(rr => rr.Contains(responseFileNameBuilder.ToString().Trim('_')));

                var fileParts = existingResource.Replace(".json", "").Split('_');
                var stringCode = fileParts.Last();

                code = Enum.Parse<HttpStatusCode>(stringCode);

                responseFileNameBuilder.Append($"_{stringCode}");
            }

            responseFileNameBuilder.Append(".json");

            var responseFileName = responseFileNameBuilder.ToString().Trim('_');
            var content = await _contentProvider.ReadAsync(responseFileName);

            var response = new HttpResponseMessage(code);
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