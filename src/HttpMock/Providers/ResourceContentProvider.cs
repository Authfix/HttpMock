using System.Reflection;

namespace Authfix.HttpMock.Providers;

public class ResourceContentProvider : IContentProvider
{
    private readonly string _responsesFolder;
    private readonly Assembly _sourceAssembly;

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

    /// <inheritdoc cref="GetListAsync"/>
    public Task<IEnumerable<string>> GetListAsync()
    {
        var resourceNames = _sourceAssembly.GetManifestResourceNames();

        return Task.FromResult<IEnumerable<string>>(resourceNames.ToList());
    }
}