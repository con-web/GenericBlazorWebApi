using System.Text.Json;
using GenericBlazorWebApi.Client;
using GenericBlazorWebApi.Shared;
using GenericBlazorWebApi.Tests.Shared.TestModels;
using WorldDomination.Net.Http;

namespace GenericBlazorWebApi.Tests.Client;

public class GenericServiceTest
{
    private readonly GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto> _service;

    public GenericServiceTest()
    {
        const string tModelName = nameof(TestModel);
        var fakeAllResponse = new ServiceResponse<List<GetTestModelDto>>
        {
            Data = new List<GetTestModelDto>
            {
                A.Dummy<GetTestModelDto>(),
                A.Dummy<GetTestModelDto>(),
                A.Dummy<GetTestModelDto>()
            }
        };

        var fakeOneResponse = new ServiceResponse<GetTestModelDto>
        {
            Data = A.Dummy<GetTestModelDto>()
        };

        var fakeAllMessage = FakeHttpMessageHandler
            .GetStringHttpResponseMessage(JsonSerializer.Serialize(fakeAllResponse));

        var fakeOneMessage = FakeHttpMessageHandler
            .GetStringHttpResponseMessage(JsonSerializer.Serialize(fakeOneResponse));

        var httpOptions = new[]
        {
            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}/all"),
                HttpResponseMessage = fakeAllMessage,
                HttpMethod = HttpMethod.Get
            },
            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}/0"),
                HttpResponseMessage = fakeOneMessage,
                HttpMethod = HttpMethod.Get
            },
            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}/0"),
                HttpResponseMessage = fakeAllMessage,
                HttpMethod = HttpMethod.Delete
            },
            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}"),
                HttpResponseMessage = fakeOneMessage,
                HttpMethod = HttpMethod.Post
            },
            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}/UniqueName"),
                HttpResponseMessage = fakeOneMessage,
                HttpMethod = HttpMethod.Post
            },

            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}/0"),
                HttpResponseMessage = fakeOneMessage,
                HttpMethod = HttpMethod.Put
            },
            new HttpMessageOptions
            {
                RequestUri = new Uri($"http://localhost/api/{tModelName}/0/UniqueName"),
                HttpResponseMessage = fakeOneMessage,
                HttpMethod = HttpMethod.Put
            }
        };

        var messageHandler = new FakeHttpMessageHandler(httpOptions);
        var httpClient = new HttpClient(messageHandler);
        httpClient.BaseAddress = new Uri("http://localhost");
        _service = new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(httpClient);
    }

    [Fact]
    public async void GetAll()
    {
        // Arrange see ctor

        // Act

        var result = await _service.GetAll();

        // Assert

        Assert.IsType<ServiceResponse<List<GetTestModelDto>>>(result);
    }

    [Fact]
    public async void Get()
    {
        // Arrange see ctor

        // Act

        var result = await _service.Get(0);

        // Assert

        Assert.IsType<ServiceResponse<GetTestModelDto>>(result);
    }

    [Fact]
    public async void Delete()
    {
        // Arrange see ctor

        // Act

        var result = await _service.Delete(0);

        // Assert

        Assert.IsType<ServiceResponse<List<GetTestModelDto>>>(result);
    }

    [Fact]
    public async void Add()
    {
        // Arrange see ctor

        // Act

        var result = await _service.Add(A.Dummy<AddTestModelDto>());

        // Assert

        Assert.IsType<ServiceResponse<GetTestModelDto>>(result);
    }

    [Fact]
    public async void Add_with_unique_identifier()
    {
        // Arrange see ctor

        // Act

        var result = await _service
            .Add(A.Dummy<AddTestModelDto>(), "UniqueName");

        // Assert

        Assert.IsType<ServiceResponse<GetTestModelDto>>(result);
    }

    [Fact]
    public async void Update()
    {
        // Arrange see ctor

        // Act

        var result = await _service.Update(A.Dummy<UpdateTestModelDto>(), 0);

        // Assert

        Assert.IsType<ServiceResponse<GetTestModelDto>>(result);
    }

    [Fact]
    public async void Update_with_unique_identifier()
    {
        // Arrange see ctor

        // Act

        var result = await _service
            .Update(A.Dummy<UpdateTestModelDto>(), 0, "UniqueName");

        // Assert

        Assert.IsType<ServiceResponse<GetTestModelDto>>(result);
    }
}