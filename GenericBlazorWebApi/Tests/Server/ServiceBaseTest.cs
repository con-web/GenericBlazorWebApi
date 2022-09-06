using GenericBlazorWebApi.Server;
using GenericBlazorWebApi.Shared;
using GenericBlazorWebApi.Tests.Shared.TestData;
using GenericBlazorWebApi.Tests.Shared.TestModels;

namespace GenericBlazorWebApi.Tests.Server;

public class GenericServiceTest
{
    private readonly IMapper _mapper;

    public GenericServiceTest()
    {
        var mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfile>(); });
        _mapper = new Mapper(mapperConfig);
    }

    [Fact]
    public async void GetAll()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();

        // Act
        var result = await service.GetAll();

        // Assert
        Assert.IsAssignableFrom<DbSet<TestModel>>(dataContext.TestModels);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.IsType<List<GetTestModelDto>>(result.Data);
        Assert.Equal(ServiceResponseCode.GetSuccess, result.ResponseCode);
    }

    [Fact]
    public async void Get_should_return_test_model_1()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();

        // Act 
        var result = await service.Get(1);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(ServiceResponseCode.GetSuccess, result.ResponseCode);
        Assert.NotNull(result.Data);
        Assert.IsType<GetTestModelDto>(result.Data);
        Assert.Equal("TestModel1", result.Data!.Name);
        Assert.Equal("TestModel1", result.Data!.UniqueName);
        Assert.NotNull(result.Data.Nullable);
    }

    [Fact]
    public async void Get_should_return_not_found()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();

        // Act 
        var result = await service.Get(99);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ServiceResponseCode.NotFound, result.ResponseCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public async void Delete_should_return_models_without_model_1()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();

        // Act 
        var result = await service.Delete(1);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(ServiceResponseCode.DeleteSuccess, result.ResponseCode);
        Assert.NotNull(result.Data);
        Assert.IsType<List<GetTestModelDto>>(result.Data);
        Assert.DoesNotContain(result.Data!, h => h.Name == "TestModel1");
    }

    [Fact]
    public async void Delete_should_return_not_found()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();

        // Act 
        var result = await service.Delete(99);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ServiceResponseCode.NotFound, result.ResponseCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public async void Add_should_return_added_model()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var newTestModel = new AddTestModelDto { Name = "TestModel4", UniqueName = "TestModel4", Nullable = "Test" };

        // Act 
        var result = await service.Add(newTestModel);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(ServiceResponseCode.AddSuccess, result.ResponseCode);
        Assert.NotNull(result.Data);
        Assert.IsType<GetTestModelDto>(result.Data);
        Assert.Equal(4, result.Data!.Id);
        Assert.Equal("TestModel4", result.Data.Name);
    }

    [Fact]
    public async void Add_with_unique_identifier_should_return_already_exists()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var newTestModel = new AddTestModelDto { Name = "TestModel1", UniqueName = "TestModel1" };

        // Act 
        var result = await service.Add(newTestModel, "UniqueName");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ServiceResponseCode.AlreadyExists, result.ResponseCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public async void Add_with_unique_identifier_should_return_added_model()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var newTestModel = new AddTestModelDto { Name = "TestModel4", UniqueName = "TestModel4" };

        // Act 
        var result = await service.Add(newTestModel, "UniqueName");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(ServiceResponseCode.AddSuccess, result.ResponseCode);
        Assert.NotNull(result.Data);
        Assert.IsType<GetTestModelDto>(result.Data);
        Assert.Equal(4, result.Data!.Id);
        Assert.Equal("TestModel4", result.Data.Name);
    }

    [Fact]
    public async void Update_should_return_updated_model()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var updateTestModel = new UpdateTestModelDto
            { Name = "TestModel1_Updated", UniqueName = "TestModel1", Nullable = "Test" };

        // Act 
        var result = await service.Update(updateTestModel, 1);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(ServiceResponseCode.UpdateSuccess, result.ResponseCode);
        Assert.NotNull(result.Data);
        Assert.IsType<GetTestModelDto>(result.Data);
        Assert.Equal(1, result.Data!.Id);
        Assert.Equal("TestModel1_Updated", result.Data.Name);
    }

    [Fact]
    public async void Update_should_return_not_found()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var updateTestModel = new UpdateTestModelDto { Name = "TestModel1", UniqueName = "TestModel1" };

        // Act 
        var result = await service.Update(updateTestModel, 99);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ServiceResponseCode.NotFound, result.ResponseCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public async void Update_with_unique_name_should_return_already_exists()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var updateTestModel = new UpdateTestModelDto { Name = "TestModel1_Updated", UniqueName = "TestModel2" };

        // Act 
        var result = await service.Update(updateTestModel, 1, "UniqueName");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ServiceResponseCode.AlreadyExists, result.ResponseCode);
        Assert.Null(result.Data);
    }

    [Fact]
    public async void Update_with_unique_name_should_return_updated_model()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var updateTestModel = new UpdateTestModelDto { Name = "TestModel1_Updated", UniqueName = "TestModel1" };

        // Act 
        var result = await service.Update(updateTestModel, 1, "UniqueName");

        // Assert
        Assert.True(result.Success);
        Assert.Equal(ServiceResponseCode.UpdateSuccess, result.ResponseCode);
        Assert.NotNull(result.Data);
        Assert.IsType<GetTestModelDto>(result.Data);
        Assert.Equal(1, result.Data!.Id);
        Assert.Equal("TestModel1_Updated", result.Data.Name);
    }

    [Fact]
    public async void Update_with_unique_name_should_return_not_found()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);
        await dataContext.Database.MigrateAsync();
        var updateTestModel = new UpdateTestModelDto { Name = "TestModel1", UniqueName = "TestModel1" };

        // Act 
        var result = await service.Update(updateTestModel, 99, "UniqueName");

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ServiceResponseCode.NotFound, result.ResponseCode);
        Assert.Null(result.Data);
    }


    [Fact]
    public async void AnyMethod_catching_exception_should_return_unknown_error()
    {
        // Arrange
        var dataContext = new TestDataContext();
        var service =
            new GenericService<TestModel, GetTestModelDto, AddTestModelDto, UpdateTestModelDto>(_mapper, dataContext);

        // Act
        var resultGetAll = await service.GetAll();
        var resultGet = await service.Get(0);
        var resultDelete = await service.Delete(0);
        var resultAdd = await service.Add(new AddTestModelDto());
        var resultAdd2 = await service.Add(new AddTestModelDto(), "string");
        var resultUpdate = await service.Update(new UpdateTestModelDto(), 0);
        var resultUpdate2 = await service.Update(new UpdateTestModelDto(), 0, "string");

        // Assert
        Assert.False(resultGetAll.Success);
        Assert.False(resultGet.Success);
        Assert.False(resultDelete.Success);
        Assert.False(resultAdd.Success);
        Assert.False(resultAdd2.Success);
        Assert.False(resultUpdate.Success);
        Assert.False(resultUpdate2.Success);
        Assert.Equal(ServiceResponseCode.UnknownError, resultGetAll.ResponseCode);
        Assert.Equal(ServiceResponseCode.UnknownError, resultGet.ResponseCode);
        Assert.Equal(ServiceResponseCode.UnknownError, resultDelete.ResponseCode);
        Assert.Equal(ServiceResponseCode.UnknownError, resultAdd.ResponseCode);
        Assert.Equal(ServiceResponseCode.UnknownError, resultAdd2.ResponseCode);
        Assert.Equal(ServiceResponseCode.UnknownError, resultUpdate.ResponseCode);
        Assert.Equal(ServiceResponseCode.UnknownError, resultUpdate2.ResponseCode);
    }
}

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<TestModel, GetTestModelDto>();
        CreateMap<AddTestModelDto, TestModel>();
        CreateMap<UpdateTestModelDto, TestModel>();
    }
}