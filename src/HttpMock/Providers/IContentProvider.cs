namespace Authfix.HttpMock.Providers;

public interface IContentProvider
{
    /// <summary>
    /// Read a resource.
    /// </summary>
    /// <param name="resource"></param>
    /// <returns></returns>
    Task<Stream> ReadAsync(string resource);

    /// <summary>
    /// Gets the list of all resources.
    /// </summary>
    /// <returns>The list of all available resources.</returns>
    Task<IEnumerable<string>> GetListAsync();
}