using GenericBlazorWebApi.Shared;
using Microsoft.AspNetCore.Mvc;

namespace GenericBlazorWebApi.Server;

/// <summary>
///     <para>
///         The <c>GenericController</c> class is a generic Web API controller which implements common CRUD operations.
///         It calls an <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> to execute database
///         transactions and sets the appropriate http response.
///     </para>
///     <example>
///         <para>
///             Implement a <c>MyModelController</c> inheriting from <c>GenericController</c> and inject your
///             <c>IMyModelService</c>
///             (which is an <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" />) into the controller:
///         </para>
///         <code>
///         <![CDATA[
///         [ApiController]
///         [Route("api")]
///         public class MyModelController : GenericController<MyModel, MyGetDto, MyAddDto, MyUpdateDto>
///         {
///             public MyModelController(IMyModelService myModelService) : base(myModelService)
///             {
///             }
///         }
///         ]]>
///         </code>
///         <para>
///             A <c>MyModelController</c> with a route directive like <c>[Route("api")]</c> provides API routes like
///             "<c>api/MyModel/all</c>" etc.
///         </para>
///     </example>
/// </summary>
/// <typeparam name="TModel">Concrete model</typeparam>
/// <typeparam name="TGetDto"><c>TModel</c> DTO for a get request</typeparam>
/// <typeparam name="TAddDto"><c>TModel</c> DTO for a post request</typeparam>
/// <typeparam name="TUpdateDto"><c>TModel</c> DTO for a put request</typeparam>
/// <seealso cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" />
[ApiController]
public class GenericController<TModel, TGetDto, TAddDto, TUpdateDto> : ControllerBase
    where TModel : class
    where TGetDto : class
    where TAddDto : class
    where TUpdateDto : class
{
    /// <summary>
    ///     The <c>IGenericService</c> which is called by the <c>GenericController</c>
    /// </summary>
    private readonly IGenericService<TModel, TGetDto, TAddDto, TUpdateDto> _service;

    /// <summary>
    ///     Initializes a new instance of the <c>GenericController</c> class
    /// </summary>
    /// <param name="service">The <c>IGenericService</c> which is called by the <c>GenericController</c></param>
    public GenericController(IGenericService<TModel, TGetDto, TAddDto, TUpdateDto> service)
    {
        _service = service;
    }

    /// <summary>
    /// Validates a <c>ServiceResponse</c> using the <see cref="ServiceResponseCode"/>
    /// </summary>
    /// <param name="response">The <c>ServiceResponse</c> to validate</param>
    /// <typeparam name="T">Datatype T</typeparam>
    /// <returns><c>OkObjectResult</c> or <c>BadRequestObjectResult</c></returns>
    public ActionResult<ServiceResponse<T>> ValidateResponse<T>(ServiceResponse<T> response)
    {
        if (response.Success) return Ok(response);
        return BadRequest(response);
    }

    /// <summary>
    ///     Gets all objects of type <c>TGetDto</c> from the <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" />.
    /// </summary>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the requested objects if the
    ///     request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpGet("[controller]/all")]
    public async Task<ActionResult<ServiceResponse<List<TGetDto>>>> GetAll()
    {
        var response = await _service.GetAll();
        return ValidateResponse(response);
    }

    /// <summary>
    ///     Gets an objects of type <c>TGetDto</c> specified by the provided <c>id</c> from the
    ///     <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" />.
    /// </summary>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the requested objects if the
    ///     request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpGet("[controller]/{id:int}")]
    public async Task<ActionResult<ServiceResponse<TGetDto>>> Get(int id)
    {
        var response = await _service.Get(id);
        return ValidateResponse(response);
    }

    /// <summary>
    ///     Calls the <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> to delete an object of type
    ///     <c>TModel</c>
    ///     specified by the provided <c>id</c> from the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    /// </summary>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the requested object of type <c>TModel</c>
    ///     from the <see cref="Microsoft.EntityFrameworkCore.DbContext" /> if the
    ///     request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpDelete("[controller]/{id:int}")]
    public async Task<ActionResult<ServiceResponse<List<TGetDto>>>> Delete(int id)
    {
        var response = await _service.Delete(id);
        return ValidateResponse(response);
    }

    /// <summary>
    ///     Calls the <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> to add an object of type
    ///     <c>TAddModel</c> to
    ///     the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    /// </summary>
    /// <param name="addDto">
    ///     The object of type <c>TAddDto</c> to add to the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </param>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the added object of type <c>TModel</c>
    ///     if the request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpPost("[controller]")]
    public async Task<ActionResult<ServiceResponse<TGetDto>>> Add(TAddDto addDto)
    {
        var response = await _service.Add(addDto);
        return ValidateResponse(response);
    }

    /// <summary>
    ///     Calls the <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> to add an object of type
    ///     <c>TAddModel</c> to
    ///     the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     Asserts that an object with a specific unique identifier (e.g. a username)
    ///     doesn't already exist in the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    /// </summary>
    /// <param name="addDto">
    ///     The object of type <c>TAddDto</c> to add to the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />
    /// </param>
    /// <param name="uniqueIdentifierPropertyName">The name of the <c>TModel</c> Property which is a unique identifier</param>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the added object of type <c>TModel</c>
    ///     if the request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpPost("[controller]/{uniqueIdentifierPropertyName}/")]
    public async Task<ActionResult<ServiceResponse<TGetDto>>> Add(TAddDto addDto, string uniqueIdentifierPropertyName)
    {
        var response = await _service.Add(addDto, uniqueIdentifierPropertyName);
        return ValidateResponse(response);
    }

    /// <summary>
    ///     Calls the <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> to update an object of type
    ///     <c>TUpdateModel</c> specified by the provided <c>id</c> in the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    /// </summary>
    /// <param name="updateDto">The object of type <c>TUpdateDto</c> to update</param>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the updated object of type <c>TModel</c>
    ///     if the request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpPut("[controller]/{id:int}")]
    public async Task<ActionResult<ServiceResponse<TGetDto>>> Update(TUpdateDto updateDto, int id)
    {
        var response = await _service.Update(updateDto, id);
        return ValidateResponse(response);
    }

    /// <summary>
    ///     Calls the <see cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> to update an object of type
    ///     <c>TUpdateModel</c> specified by the provided <c>id</c> in the
    ///     <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    ///     Asserts that an object with a specific unique identifier (e.g. a username)
    ///     doesn't already exist in the <see cref="Microsoft.EntityFrameworkCore.DbContext" />.
    /// </summary>
    /// <param name="updateDto">The object of type <c>TUpdateDto</c> to update</param>
    /// <param name="id">The <c>id</c> of the specified object</param>
    /// <param name="uniqueIdentifierPropertyName">The name of the <c>TModel</c> Property which is a unique identifier</param>
    /// <returns>
    ///     An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult" /> containing the updated object of type <c>TModel</c>
    ///     if the request is successful, else <see cref="Microsoft.AspNetCore.Mvc.BadRequestObjectResult" />
    /// </returns>
    [HttpPut("[controller]/{id:int}/{uniqueIdentifierPropertyName}/")]
    public async Task<ActionResult<ServiceResponse<TGetDto>>> Update(TUpdateDto updateDto, int id,
        string uniqueIdentifierPropertyName)
    {
        var response = await _service.Update(updateDto, id, uniqueIdentifierPropertyName);
        return ValidateResponse(response);
    }
}