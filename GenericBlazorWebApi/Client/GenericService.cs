using System.Net.Http.Json;
using GenericBlazorWebApi.Shared;

namespace GenericBlazorWebApi.Client;

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
    private readonly HttpClient _httpClient;


    private readonly string _tModelName;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GenericService{TModel,TGetDto,TAddDto,TUpdateDto}" /> class.
    /// </summary>
    /// <param name="httpClient">HttpClient to inject</param>
    public GenericService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _tModelName = typeof(TModel).Name;
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.GetAll()" />
    public async Task<ServiceResponse<List<TGetDto>>> GetAll()
    {
        var response = await _httpClient
            .GetFromJsonAsync<ServiceResponse<List<TGetDto>>>($"api/{_tModelName}/all");
        return CheckApiResponse(response);
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Get(int)" />
    public async Task<ServiceResponse<TGetDto>> Get(int id)
    {
        var response = await _httpClient
            .GetFromJsonAsync<ServiceResponse<TGetDto>>($"api/{_tModelName}/{id}");
        return CheckApiResponse(response);
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Delete(int)" />
    public async Task<ServiceResponse<List<TGetDto>>> Delete(int id)
    {
        var result = await _httpClient
            .DeleteAsync($"api/{_tModelName}/{id}");
        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<List<TGetDto>>>();
        return CheckApiResponse(response);
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Add(TAddDto)" />
    public async Task<ServiceResponse<TGetDto>> Add(TAddDto addDto)
    {
        var result = await _httpClient
            .PostAsJsonAsync($"api/{_tModelName}", addDto);
        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<TGetDto>>();
        return CheckApiResponse(response);
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Add(TAddDto, string)" />
    public async Task<ServiceResponse<TGetDto>> Add(TAddDto addDto, string uniqueIdentifierPropertyName)
    {
        var result = await _httpClient
            .PostAsJsonAsync($"api/{_tModelName}/{uniqueIdentifierPropertyName}", addDto);
        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<TGetDto>>();
        return CheckApiResponse(response);
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Update(TUpdateDto, int)" />
    public async Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id)
    {
        var result = await _httpClient
            .PutAsJsonAsync($"api/{_tModelName}/{id}", updateDto);
        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<TGetDto>>();
        return CheckApiResponse(response);
    }

    /// <inheritdoc cref="IGenericService{TModel,TGetDto,TAddDto,TUpdateDto}.Update(TUpdateDto, int, string)" />
    public async Task<ServiceResponse<TGetDto>> Update(TUpdateDto updateDto, int id,
        string uniqueIdentifierPropertyName)
    {
        var result = await _httpClient
            .PutAsJsonAsync($"api/{_tModelName}/{id}/{uniqueIdentifierPropertyName}", updateDto);
        var response = await result.Content.ReadFromJsonAsync<ServiceResponse<TGetDto>>();
        return CheckApiResponse(response);
    }

    private static ServiceResponse<T> CheckApiResponse<T>(ServiceResponse<T>? response)
    {
        return response ?? new ServiceResponse<T> { ResponseCode = ServiceResponseCode.NoApiResponse };
    }
}