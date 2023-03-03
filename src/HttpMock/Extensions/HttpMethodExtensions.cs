namespace Authfix.HttpMock.Extensions;

internal static class HttpMethodExtensions
{
    public static int ToDefaultResultCode(this HttpMethod method)
    {
        return method switch
        {
            { } when method == HttpMethod.Post => 201,
            _ => 200
        };
    }
}