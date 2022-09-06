using GenericBlazorWebApi.Server;
using GenericBlazorWebApi.Shared;
using GenericBlazorWebApi.Tests.Shared.TestModels;

namespace GenericBlazorWebApi.Tests.Server;

public class GenericControllerTest
{
    private readonly GenericController<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>
        _controller;

    private readonly IGenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto> _service;

    public GenericControllerTest()
    {
        _service = A.Fake<IGenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>>();
        _controller =
            new GenericController<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_service);
    }

    [Fact]
    public async void GetAll()
    {
        // Arrange

        var fakeResponse = A.Dummy<ServiceResponse<List<GetTestModelDto>>>();
        A.CallTo(() => _service.GetAll()).Returns(fakeResponse);

        // Act

        var result = await _controller.GetAll();

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<List<GetTestModelDto>>>>(result);
    }

    [Fact]
    public async void Get()
    {
        // Arrange

        var fakeResponse = A.Dummy<ServiceResponse<GetTestModelDto>>();
        A.CallTo(() => _service.Get(A<int>.Ignored)).Returns(fakeResponse);

        // Act

        var result = await _controller.Get(0);

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<GetTestModelDto>>>(result);
    }

    [Fact]
    public async void Delete()
    {
        // Arrange

        var fakeResponse = A.Dummy<ServiceResponse<List<GetTestModelDto>>>();
        A.CallTo(() => _service.Delete(A<int>.Ignored)).Returns(fakeResponse);

        // Act

        var result = await _controller.Delete(0);

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<List<GetTestModelDto>>>>(result);
    }

    [Fact]
    public async void Add()
    {
        // Arrange
        var fakeAddTestModelDto = A.Dummy<AddTestModelDto>();
        var fakeResponse = A.Dummy<ServiceResponse<GetTestModelDto>>();
        A.CallTo(() => _service.Add(fakeAddTestModelDto)).Returns(fakeResponse);

        // Act

        var result = await _controller.Add(fakeAddTestModelDto);

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<GetTestModelDto>>>(result);
    }

    [Fact]
    public async void Add_with_unique_name()
    {
        // Arrange
        var fakeAddTestModelDto = A.Dummy<AddTestModelDto>();
        var fakeResponse = A.Dummy<ServiceResponse<GetTestModelDto>>();
        A.CallTo(() => _service.Add(fakeAddTestModelDto, A<string>.Ignored)).Returns(fakeResponse);

        // Act
        var result = await _controller.Add(fakeAddTestModelDto, "test");

        // Assert
        Assert.IsType<ActionResult<ServiceResponse<GetTestModelDto>>>(result);
    }

    [Fact]
    public async void Update()
    {
        // Arrange
        var fakeUpdateTestModelDto = A.Dummy<UpdateTestModelDto>();
        var fakeResponse = A.Dummy<ServiceResponse<GetTestModelDto>>();
        A.CallTo(() => _service.Update(fakeUpdateTestModelDto, A<int>.Ignored)).Returns(fakeResponse);

        // Act

        var result = await _controller.Update(fakeUpdateTestModelDto, 0);

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<GetTestModelDto>>>(result);
    }

    [Fact]
    public async void Update_with_response()
    {
        // Arrange
        var fakeUpdateTestModelDto = A.Dummy<UpdateTestModelDto>();
        var fakeResponse = A.Dummy<ServiceResponse<GetTestModelDto>>();
        A.CallTo(() => _service.Update(fakeUpdateTestModelDto, A<int>.Ignored, A<string>.Ignored))
            .Returns(fakeResponse);

        // Act

        var result = await _controller.Update(fakeUpdateTestModelDto, 0, "Test");

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<GetTestModelDto>>>(result);
    }


    [Fact]
    public async void ValidateResponse()
    {
        // Arrange
        var fakeUpdateTestModelDto = A.Dummy<UpdateTestModelDto>();
        var fakeResponse = A.Dummy<ServiceResponse<GetTestModelDto>>();
        fakeResponse.ResponseCode = ServiceResponseCode.UnknownError;
        A.CallTo(() => _service.Update(fakeUpdateTestModelDto, A<int>.Ignored)).Returns(fakeResponse);

        // Act

        var result = await _controller.Update(fakeUpdateTestModelDto, 0);

        // Assert

        Assert.IsType<ActionResult<ServiceResponse<GetTestModelDto>>>(result);
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}