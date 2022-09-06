using AutoMapper;
using GenericBlazorWebApi.Shared;
using Microsoft.EntityFrameworkCore;

namespace GenericBlazorWebApi.Server;

/// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}" />
public class
    GenericService<
        TModel,
        TGetDto,
        TAddDto,
        TUpdateDto
    > : IGenericService<
        TModel,
        TGetDto,
        TAddDto,
        TUpdateDto
    >
    where TModel : class
    where TGetDto : class
    where TAddDto : class
    where TUpdateDto : class
{
    private readonly DbContext _dataContext;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the <c>GenericService</c> class.
    /// </summary>
    /// <param name="mapper">The <see cref="IMapper" /> instance to inject</param>
    /// <param name="dataContext">The <see cref="DbContext" /> to inject</param>
    public GenericService(IMapper mapper, DbContext dataContext)
    {
        _mapper = mapper;
        _dataContext = dataContext;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.GetAll()" />
    public async Task<ServiceResponse<List<TGetDto>>> GetAll()
    {
        var response = new ServiceResponse<List<TGetDto>>();

        try
        {
            response.Data = await _dataContext.Set<TModel>()
                .Select(i => _mapper.Map<TGetDto>(i))
                .ToListAsync();
            response.ResponseCode = ServiceResponseCode.GetSuccess;
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Get(int)" />
    public async Task<ServiceResponse<TGetDto>> Get(int id)
    {
        var response = new ServiceResponse<TGetDto>();

        try
        {
            var objects = await _dataContext.Set<TModel>().ToListAsync();
            var singleObject = objects
                .Find(i => Utility.GetPropertyValue<int>(i, "Id") == id);

            if (singleObject == null)
            {
                response.ResponseCode = ServiceResponseCode.NotFound;
            }
            else
            {
                response.Data = _mapper.Map<TGetDto>(singleObject);
                response.ResponseCode = ServiceResponseCode.GetSuccess;
            }
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Delete(int)" />
    public async Task<ServiceResponse<List<TGetDto>>> Delete(int id)
    {
        var response = new ServiceResponse<List<TGetDto>>();

        try
        {
            var objects = await _dataContext.Set<TModel>().ToListAsync();
            var singleObject = objects
                .Find(i => Utility.GetPropertyValue<int>(i, "Id") == id);

            if (singleObject == null)
            {
                response.ResponseCode = ServiceResponseCode.NotFound;
            }
            else
            {
                _dataContext.Set<TModel>().Remove(singleObject);
                await _dataContext.SaveChangesAsync();
                response.Data = await _dataContext.Set<TModel>()
                    .Select(i => _mapper.Map<TGetDto>(i))
                    .ToListAsync();
                response.ResponseCode = ServiceResponseCode.DeleteSuccess;
            }
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Add(TAddDto)" />
    public async Task<ServiceResponse<TGetDto>> Add(TAddDto addDto)
    {
        var response = new ServiceResponse<TGetDto>();

        try
        {
            var newSingleObject = _mapper.Map<TModel>(addDto);
            _dataContext.Set<TModel>().Add(newSingleObject);
            await _dataContext.SaveChangesAsync();

            response.Data = _mapper.Map<TGetDto>(newSingleObject);
            response.ResponseCode = ServiceResponseCode.AddSuccess;
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Add(TAddDto, string)" />
    public async Task<ServiceResponse<TGetDto>> Add(TAddDto addDto, string uniqueIdentifierPropertyName)
    {
        var response = new ServiceResponse<TGetDto>();

        try
        {
            var addDtoUniqueIdentifier = Utility.GetPropertyValue<string>(addDto, uniqueIdentifierPropertyName);

            if (GetIdByUniqueIdentifier(uniqueIdentifierPropertyName, addDtoUniqueIdentifier) != null)
            {
                response.ResponseCode = ServiceResponseCode.AlreadyExists;
            }
            else
            {
                var newSingleObject = _mapper.Map<TModel>(addDto);
                _dataContext.Set<TModel>().Add(newSingleObject);
                await _dataContext.SaveChangesAsync();

                response.Data = _mapper.Map<TGetDto>(newSingleObject);
                response.ResponseCode = ServiceResponseCode.AddSuccess;
            }
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Update(TUpdateDto, int)" />
    public async Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id)
    {
        var response = new ServiceResponse<TGetDto>();

        try
        {
            var objects = await _dataContext.Set<TModel>().ToListAsync();
            var singleObject = objects
                .Find(i => Utility.GetPropertyValue<int>(i, "Id") == id);

            if (singleObject == null)
            {
                response.ResponseCode = ServiceResponseCode.NotFound;
            }
            else
            {
                _mapper.Map(updateDto, singleObject);
                await _dataContext.SaveChangesAsync();

                response.Data = _mapper.Map<TGetDto>(singleObject);
                response.ResponseCode = ServiceResponseCode.UpdateSuccess;
            }
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Update(TUpdateDto, int, string)" />
    public async Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id,
        string uniqueIdentifierPropertyName)
    {
        var response = new ServiceResponse<TGetDto>();

        try
        {
            var updateDtoUniqueIdentifier = Utility.GetPropertyValue<string>(updateDto, uniqueIdentifierPropertyName);
            var existingSingleObjectId =
                GetIdByUniqueIdentifier(uniqueIdentifierPropertyName, updateDtoUniqueIdentifier);


            var objects = await _dataContext.Set<TModel>().ToListAsync();
            var singleObject = objects
                .Find(i => Utility.GetPropertyValue<int>(i, "Id") == id);

            if (singleObject == null)
            {
                response.ResponseCode = ServiceResponseCode.NotFound;
            }
            else if (existingSingleObjectId != null &&
                     existingSingleObjectId != Utility.GetPropertyValue<int>(singleObject, "Id"))
            {
                response.ResponseCode = ServiceResponseCode.AlreadyExists;
            }
            else
            {
                _mapper.Map(updateDto, singleObject);
                await _dataContext.SaveChangesAsync();

                response.Data = _mapper.Map<TGetDto>(singleObject);
                response.ResponseCode = ServiceResponseCode.UpdateSuccess;
            }
        }
        catch (Exception e)
        {
            response.ResponseCode = ServiceResponseCode.UnknownError;
            response.ResponseMessage = e.Message;
        }

        return response;
    }


    private int? GetIdByUniqueIdentifier(
        string uniqueIdentifierPropertyName,
        string uniqueIdentifier
    )
    {
        var objects = _dataContext.Set<TModel>().ToList();
        var singleObject = objects.Find(i =>
            uniqueIdentifier == Utility.GetPropertyValue<string>(i, uniqueIdentifierPropertyName));
        return singleObject == null ? null : Utility.GetPropertyValue<int?>(singleObject, "Id");
    }
}