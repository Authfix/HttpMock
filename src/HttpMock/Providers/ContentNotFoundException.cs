namespace Authfix.HttpMock.Providers;

[Serializable]
public class ContentNotFoundException : Exception
{
    /// <summary>
    /// Initialize a new <see cref="ContentNotFoundException"/>.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public ContentNotFoundException(string message) : base(message)
    {
        
    }
}