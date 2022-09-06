namespace GenericBlazorWebApi.Shared.Exceptions;

/// <summary>
///     The <c>PropertyNotFoundException</c> is thrown if you try to get an objects property which doesn't exist.
/// </summary>
public class PropertyNotFoundException : Exception
{
    /// <summary>
    ///     Initializes the <c>PropertyNotFoundException</c>.
    /// </summary>
    public PropertyNotFoundException()
    {
    }

    /// <summary>
    ///     Initializes the <c>PropertyNotFoundException</c>.
    /// </summary>
    /// <param name="message">exception message</param>
    public PropertyNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    ///     Initializes the <c>PropertyNotFoundException</c>.
    /// </summary>
    /// <param name="message">exception message</param>
    /// <param name="inner">inner exception</param>
    public PropertyNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
    }
}