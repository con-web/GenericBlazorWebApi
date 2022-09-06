using System.Text.Json.Serialization;

namespace GenericBlazorWebApi.Shared;

/// <summary>
///     The service response code provides detailed information about the success of the request.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ServiceResponseCode
{
    /// <summary>
    ///     The <c>Default</c> response code stands for an unspecified successful response.
    /// </summary>
    Default = 0,

    /// <summary>
    ///     The get success response code stands for a successful response to a get request.
    /// </summary>
    GetSuccess = 1,

    /// <summary>
    ///     The add success response code stands for a successful response to a post request.
    /// </summary>
    AddSuccess = 2,

    /// <summary>
    ///     The update success response code stands for a successful response to a put request.
    /// </summary>
    UpdateSuccess = 3,

    /// <summary>
    ///     The delete success response code stands for a successful response to a delete request.
    /// </summary>
    DeleteSuccess = 4,


    /// <summary>
    ///     The not found response code stands for an unsuccessful response to a request
    ///     because an object with the requested id does not exist in the data context.
    /// </summary>
    NotFound = 5,

    /// <summary>
    ///     The already exists response code stands for an unsuccessful response to a get request, post request or a put
    ///     request
    ///     due to an object already existing in the data context, which has the requested id or the requested unique name.
    /// </summary>
    AlreadyExists = 6,

    /// <summary>
    ///     The unknown error response code stands for an unsuccessful response due to an unknown exception.
    /// </summary>
    UnknownError = 7,

    /// <summary>
    ///     The no api response response code stands for an unsuccessful response due to a not responding server.
    ///     Is intended for use by the client.
    /// </summary>
    NoApiResponse = 8
}