using GenericBlazorWebApi.Shared.Exceptions;

namespace GenericBlazorWebApi.Tests.Shared;

public class ExceptionsTest
{
    [Fact]
    public void PropertyNotFoundException_should_throw()
    {
        // Arrange

        // Act and Assert
        Assert.ThrowsAsync<PropertyNotFoundException>(() =>
            throw new PropertyNotFoundException());

        Assert.ThrowsAsync<PropertyNotFoundException>(() =>
            throw new PropertyNotFoundException("with message"));

        Assert.ThrowsAsync<PropertyNotFoundException>(() =>
            throw new PropertyNotFoundException("with inner", new Exception()));
    }
}