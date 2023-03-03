using System.Net;

namespace Authfix.HttpMock.Extensions;

internal static class HttpMethodExtensions
{
    public static HttpStatusCode ToDefaultResultCode(this HttpMethod method)
    {
        return method switch
        {
            { } when method == HttpMethod.Post => HttpStatusCode.Created,
            _ => HttpStatusCode.OK
        };
    }
}