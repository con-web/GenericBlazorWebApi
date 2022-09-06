using AutoMapper;
using GenericBlazorWebApi.Shared;
using Microsoft.EntityFrameworkCore;

namespace GenericBlazorWebApi.Server;

/// <summary>
///     The <c>GenericService</c> executes the common CRUD database transactions requested by the
///     <c>GenericController</c>
///     for any model and corresponding DTO of the given types <c>TModel</c>, <c>TGetDto</c>, <c>TAddDto</c> and
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
///             public MyModelService(IMapper mapper, DbContext dataContext) : base(mapper, dataContext)
///                 {
///                 }
///         }
///         ]]>
///         </code>
///         In your server's Program.cs:
///        <code>
///         <![CDATA[builder.Services.AddScoped<IMyModelService, MyModelService>();]]>
///         </code>
///         <remarks>
///             Register your <c>DbContext</c> implementation as an injectable service to make this work out of the box.
///             Else, in the
///             <c>ctor</c> of this class change the type <c>DbContext</c> to your implementation of <c>DbContext</c>.
///         </remarks>
///     </example>
/// </summary>
/// <typeparam name="TModel">Concrete model</typeparam>
/// <typeparam name="TGetDto"><c>TModel</c> DTO for a get request</typeparam>
/// <typeparam name="TAddDto"><c>TModel</c> DTO for a post request</typeparam>
/// <typeparam name="TUpdateDto"><c>TModel</c> DTO for a put request</typeparam>
/// <seealso cref="GenericController{TModel,TGetDto,TAddDto,TUpdateDto}" />
/// <seealso cref="IMapper" />
/// <seealso cref="DbContext" />
// ReSharper disable once UnusedTypeParameter
public interface IGenericService<TModel, TGetDto, TAddDto, TUpdateDto>
{
    /// <summary>
    ///     Gets all objects of type <c>TModel</c> from the <see cref="Microsoft.EntityFrameworkCore.DbContext" /> and maps
    ///     them to <c>TGetDto</c>.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.GetSuccess" />.
    /// </summary>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the requested objects</returns>
    Task<ServiceResponse<List<TGetDto>>> GetAll();

    /// <summary>
    ///     Gets an object of type <c>TModel</c> specified by the provided <c>id</c> from the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" /> and
    ///     maps it to <c>TGetDto</c>.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.GetSuccess" />.
    /// </summary>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the requested object</returns>
    Task<ServiceResponse<TGetDto>> Get(int id);

    /// <summary>
    ///     Deletes an object of type <c>TModel</c> specified by the provided <c>id</c> from the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.DeleteSuccess" />.
    /// </summary>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>
    ///     The <see cref="ServiceResponse{T}" /> containing the remaining objects of type <c>TModel</c> from the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </returns>
    Task<ServiceResponse<List<TGetDto>>> Delete(int id);

    /// <summary>
    ///     Maps an object of type <c>TAddDto</c> to <c>TModel</c> and adds it to the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.AddSuccess" />.
    /// </summary>
    /// <param name="addDto">
    ///     The object of type <c>TAddDto</c> to add to the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the added object. </returns>
    Task<ServiceResponse<TGetDto>> Add(TAddDto addDto);

    /// <summary>
    ///     Maps an object of type <c>TAddDto</c> to <c>TModel</c> and adds it to the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     Asserts that an object with a specific unique identifier (e.g. a username)
    ///     doesn't already exist in the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.AddSuccess" />.
    /// </summary>
    /// <param name="addDto">
    ///     The object of type <c>TAddDto</c> to add to the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </param>
    /// <param name="uniqueIdentifierPropertyName">The name of the <c>TModel</c> Property which is a unique identifier</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the added object. </returns>
    Task<ServiceResponse<TGetDto>> Add(TAddDto addDto, string uniqueIdentifierPropertyName);

    /// <summary>
    ///     Maps an object of type <c>TModel</c> specified by the provided <c>id</c> to <c>TUpdateDto</c> and updates the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.UpdateSuccess" />.
    /// </summary>
    /// <param name="updateDto">The object of type <c>TUpdateDto</c> to update</param>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the added object</returns>
    Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id);

    /// <summary>
    ///     Maps an object of Type <c>TModel</c> specified by the provided <c>id</c> to <c>TUpdateDto</c> and updates the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     Asserts that an object with a specific unique Identifier (e.g. a username)
    ///     doesn't already exist in the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     If successful, it sets the <see cref="ServiceResponseCode" />
    ///     to <see cref="ServiceResponseCode.UpdateSuccess" />.
    /// </summary>
    /// <param name="updateDto">The object of type <c>TUpdateDto</c> to update</param>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <param name="uniqueIdentifierPropertyName">The name of the <c>TModel</c> Property which is a unique identifier</param>
    /// <returns>The <see cref="ServiceResponse{T}" /> containing the updated object. </returns>
    Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id, string uniqueIdentifierPropertyName);
}