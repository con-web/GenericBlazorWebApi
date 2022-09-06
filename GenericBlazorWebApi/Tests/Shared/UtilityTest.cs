using GenericBlazorWebApi.Shared;
using GenericBlazorWebApi.Shared.Exceptions;
using GenericBlazorWebApi.Tests.Shared.TestModels;

namespace GenericBlazorWebApi.Tests.Shared;

public class UtilityTest
{
    [Fact]
    public void GetPropertyValue_should_return_property()
    {
        // Arrange
        var testObject = new TestModel { Name = "Test" };

        // Act

        var result = Utility.GetPropertyValue<string>(testObject, "Name");

        // Assert
        Assert.Equal("Test", result);
    }

    [Fact]
    public void GetPropertyValue_should_return_property_not_found_exception()
    {
        // Arrange
        var testObject = new TestModel();

        // Act and Assert
        Assert.Throws<PropertyNotFoundException>(() =>
            Utility.GetPropertyValue<string>(testObject, "Any"));
    }

    [Fact]
    public void GetPropertyValue_should_return_null_reference_exception()
    {
        // Arrange
        var testObject = new TestModel { Nullable = null };

        // Act and Assert
        Assert.Throws<NullReferenceException>(() =>
            Utility.GetPropertyValue<string>(testObject, "Nullable"));
    }
}