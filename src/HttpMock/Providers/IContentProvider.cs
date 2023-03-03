namespace Authfix.HttpMock.Providers;

public interface IContentProvider
{
    /// <summary>
    /// Read a resource.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    Task<Stream> ReadAsync(string resource);
}