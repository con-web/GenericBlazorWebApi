# GenericBlazorWebApi

This project provides some generic helpers for the ASP.NET Core hosted Blazor Webassembly template structure.
It provides a generic api controller as well as server side and client side generic services following the repository
pattern.

### Dependencies

- AutoMapper.Extensions.Microsoft.DependencyInjection >= 11.0.0
- Microsoft.Asp.NetCore.Mvc.Core >= 2.2.5
- Microsoft.EntityFrameworkCore >= 6.0.8

See also dependencies for the tests in the ```GenericBlazorWepApi.Tests.csproj```.

## Basic Usage

It is assumed that you already have done the following steps:

- initialized the ASP.NET Core hosted Blazor Webassembly template
- implemented a data model and some DTOs for get, put, post methods
- for the server project:
    - set up automapper with a profile mapping the model and the DTOs
    - implemented a DbContext with proper connection string to a db
    - added migrations and updated db

Following CRUD operations for a specific model are supported:

- getall -> gets all objects
- get -> gets object by id
- delete -> deletes object by id
- add -> adds object
- update -> edits / updates object

### Server

#### Get the service going

```csharp
using GenericBlazorWebApi.Server;

public interface IMyModelService : IGenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>  
{  
}  

public class MyModelService : GenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>, IMyModelService  
{  
	public MyModelService(IMapper mapper, DbContext dataContext) : base(mapper, dataContext)  
    {  
    }
}
```

Register the service in the program.cs:

```csharp
builder.Services.AddScoped<IMyModelService, MyModelService>();
```

If you want this to work out of the box, you'll have to register your implementation of DbContext as a Service too:

```csharp
builder.Services.AddScoped<DbContext, MyDataContext>();
```

If you don't want to do that, you can also change the constructor of MyModelService:

```csharp
using GenericBlazorWebApi.Server

public class MyModelService : GenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>, IMyModelService  
{  
	public MyModelService(IMapper mapper, MyDataContext dataContext) : base(mapper, dataContext)  
    {

    }
}
```

#### Inject the service into the generic controller

```csharp
using GenericBlazorWebApi.Server;

[ApiController]
[Route("api")] // optional
public class MyModelController : GenericController<MyModel, MyGetDto, MyAddDto, MyUpdateDto>
{
    public MyModelController(IMyModelService myModelService) : base(myModelService)
    {

    }
}
```

#### Build and run the server

By now you should have access to the api controller via the specified route (e.g. "api/get/all", use swagger to check
out the available routes).

### Client

#### Get the service going

```csharp
using GenericBlazorWebApi.Client;

public interface IMyModelService : IGenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>  
{  
}  

public class MyModelService : GenericService<MyModel, MyGetDto, MyAddDto, MyUpdateDto>, IMyModelService  
{  
	public MyModelService(HttpClient httpClient) : base(httpClient)
    {

    }
}
```

Register the service in the program.cs:

```csharp
builder.Services.AddScoped<IMyModelService, MyModelService>();
```

#### Build and run server and client

By now you should have access to the api controller via the specified routes. Also you should be able to access the api
controller from a blazor page with the client service being injected to that page.

## Advanced usage

Feel free to implement additional methods in the MyModelController and in the server MyModelService (don't forget to add the methods
to the IMyModelService interface):

```csharp
using GenericBlazorWebApi.Server;

[ApiController]
[Route("api")]
public class MyModelController : GenericController<MyModel, MyModelGetDto, MyModelAddDto, MyModelUpdateDto>
{
    private readonly IMyModelService _service;
    public MyModelController(IMyModelService service) : base(service)
    {
        _service = service;
    }

    [HttpPut("[controller]/{id:int}/update/{someCrazyAttribute}")]
    public async Task<ActionResult<ServiceResponse<MyModelGetDto>>> ChangeSomeCrazyAttribute(int id, string someCrazyAttribute)
    {
        var response = await _service.ChangeSomeCrazyAttribute(id, someCrazyAttribute);
        if (response.Success) return Ok(response);
        return BadRequest(response);
    }
}
...
public interface IMyModelService : IGenericService<MyModel, MyModelGetDto, MyModelAddDto, MyModelUpdateDto>
{
    Task<ServiceResponse<MyModelGetDto>> ChangeSomeCrazyAttribute(int id, string someCrazyAttribute);
    
}
...
public class MyModelService : GenericService<MyModel, MyModelGetDto, MyModelAddDto, MyModelUpdateDto>, IMyModelService
{
    private readonly DbContext _datacontext;
    private readonly IMapper _mapper;
    
    public MyModelService(IMapper mapper, DbContext dataContext) : base(mapper, dataContext)
    {
        _datacontext = dataContext;
        _mapper = mapper;
    }
    
    public async Task<ServiceResponse<MyModelGetDto>> ChangeSomeCrazyAttribute(int id, string someCrazyAttribute)
    {
        var response = new ServiceResponse<MyModelGetDto>();
        try
        {
            var myModel = await _datacontext.Set<MyModel>().FirstOrDefaultAsync(s => s.Id == id);
            myModel!.SomeCrazyAttribute = someCrazyAttribute;
            await _datacontext.SaveChangesAsync();
            response.Data = _mapper.Map<MyModelGetDto>(myModel);
            response.ResponseCode = ServiceResponseCode.UpdateSuccess;
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }
}
```

Add methods to the client MyModelService as described above.

