using System.Reflection;

namespace Authfix.HttpMock.Providers;

public class ResourceContentProvider : IContentProvider
{
    private readonly Assembly _sourceAssembly;
    private readonly string _responsesFolder;
    
    /// <summary>
    /// Initialize a new <see cref="ResourceContentProvider"/>.
    /// </summary>
    /// <param name="sourceAssembly">The source assembly where resource are stored.</param>
    /// <param name="responsesFolder">The default folder where responses are stored.</param>
    /// <exception cref="ArgumentNullException">If an argument is null.</exception>
    public ResourceContentProvider(Assembly sourceAssembly, string responsesFolder)
    {
        _sourceAssembly = sourceAssembly ?? throw new ArgumentNullException(nameof(sourceAssembly));
        _responsesFolder = responsesFolder ?? throw new ArgumentNullException(nameof(responsesFolder));
    }
    
    /// <inheritdoc cref="ReadAsync"/>
    public Task<Stream> ReadAsync(string resource)
    {
        var fullQualifiedResource = string.Join('.', _sourceAssembly.GetName().Name, _responsesFolder, resource);

        var resourceStream = _sourceAssembly.GetManifestResourceStream(fullQualifiedResource);

        if (resourceStream == null)
        {
            throw new ContentNotFoundException($"Resource {resource} not found");
        }

        return Task.FromResult(resourceStream);
    }
}