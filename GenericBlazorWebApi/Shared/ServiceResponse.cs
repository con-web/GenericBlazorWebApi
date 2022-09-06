using System.Text.Json.Serialization;

namespace GenericBlazorWebApi.Shared;

/// <summary>
///     The service response class wraps the actual response and provides a <see cref="ResponseCode" />
/// </summary>
public class ServiceResponse<T>
{
    /// <summary>
    ///     Gets or sets the value of the response body.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    ///     Gets or sets the value of the <c>ResponseCode</c>
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ServiceResponseCode ResponseCode { get; set; } = 0;

    /// <summary>
    ///     The <c>ResponseMessage </c> provides additional detail about the response, e.g. an exception message.
    /// </summary>
    public string ResponseMessage { get; set; } = string.Empty;

    /// <summary>
    ///     Success flag depending on the <see cref="ResponseCode" /> of a <c>ServiceResponse</c>.
    /// </summary>
    public bool Success => (int)ResponseCode < 5;
}