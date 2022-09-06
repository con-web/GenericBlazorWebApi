using GenericBlazorWebApi.Shared;

namespace GenericBlazorWebApi.Client;

/// <summary>
///     The <c>GenericService</c> lets the client send http requests for common CRUD operations to a
///     server's <c>ApiController</c> for any model and corresponding DTO of the given types <c>TModel</c>,
///     <c>TGetDto</c>, <c>TAddDto</c> and
///     <c>TUpdateDto</c>.
///     <example> In your server project:
///         <code>
///         <![CDATA[
///         IMyModelService : IGenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>
///         {
///         }
///         ]]>
///         </code>
///         <code>
///         <![CDATA[
///         MyModelService : GenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>, IMyModelService
///         {
///             public MyModelService(HttpClient httpClient) : base(httpClient)
///                 {
///                 }
///         }
///         ]]>
///         </code>
///         In your server's Program.cs:
///        <code>
///         <![CDATA[builder.Services.AddScoped<IMyModelService, MyModelService>();]]>
///         </code>
///     </example>
/// </summary>
/// <typeparam name="TModel">Concrete model</typeparam>
/// <typeparam name="TGetDto"><c>TModel</c> DTO for a get request</typeparam>
/// <typeparam name="TAddDto"><c>TModel</c> DTO for a post request</typeparam>
/// <typeparam name="TUpdateDto"><c>TModel</c> DTO for a put request</typeparam>
// ReSharper disable once UnusedTypeParameter
public interface IGenericService<TModel, TGetDto, TAddDto, TUpdateDto>
{
    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to get all objects of type <c>TGetDto</c>.
    /// </summary>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the requested objects</returns>
    Task<ServiceResponse<List<TGetDto>>> GetAll();

    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to get an object of type <c>TGetDto</c> specified by the provided <c>id</c>.
    /// </summary>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the requested object</returns>
    Task<ServiceResponse<TGetDto>> Get(int id);

    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to delete an object of type <c>TModel</c> specified by the provided <c>id</c>.
    /// </summary>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>
    ///     The <see cref="ServiceResponse{T}" /> containing the remaining objects of type <c>TModel</c> from the
    ///     <c>DbContext</c>
    /// </returns>
    Task<ServiceResponse<List<TGetDto>>> Delete(int id);

    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to add an object of type <c>TAddDto</c>.
    /// </summary>
    /// <param name="addDto">
    ///     The object of type <c>TAddDto</c> to add to the
    ///     <c>DbContext</c>
    /// </param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the added object. </returns>
    Task<ServiceResponse<TGetDto>> Add(TAddDto addDto);

    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to add an object of type <c>TAddDto</c> specified by the provided <c>id</c>.
    ///     Asserts, that an object with a specific unique Identifier (e.g. a username)
    ///     doesn't already exist in the <c>DbContext</c>.
    /// </summary>
    /// <param name="addDto">
    ///     The object of type <c>TAddDto</c> to add to the
    ///     <c>DbContext</c>
    /// </param>
    /// <param name="uniqueIdentifierPropertyName">The name of the <c>TModel</c> Property which is a unique identifier</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the added object. </returns>
    Task<ServiceResponse<TGetDto>> Add(TAddDto addDto, string uniqueIdentifierPropertyName);

    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to update an object of type <c>TUpdateDto</c> specified by the provided <c>id</c>.
    /// </summary>
    /// <param name="updateDto">The object of type <c>TUpdateDto</c> to update</param>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the added object</returns>
    Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id);

    /// <summary>
    ///     Requests the
    ///     <c>ApiController</c>
    ///     to update an object of type <c>TUpdateDto</c> specified by the provided <c>id</c>.
    ///     Asserts, that object with a specific unique Identifier (e.g. a username)
    ///     doesn't already exist in the <c>DbContext</c>.
    /// </summary>
    /// <param name="updateDto">The object of type <c>TUpdateDto</c> to update</param>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <param name="uniqueIdentifierPropertyName">The name of the <c>TModel</c> Property which is a unique identifier</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the updated object. </returns>
    Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id, string uniqueIdentifierPropertyName);
}
