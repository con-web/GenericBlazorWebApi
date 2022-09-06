using GenericBlazorWebApi.Shared.Exceptions;

namespace GenericBlazorWebApi.Shared;

/// <summary>
///     The utility class provides generic utility methods.
/// </summary>
public static class Utility
{
    /// <summary>
    ///     Gets the property value of an object using the specified property name.
    /// </summary>
    /// <typeparam name="T">The property type</typeparam>
    /// <param name="obj">The object</param>
    /// <param name="propertyName">The property name</param>
    /// <exception cref="PropertyNotFoundException">{obj} has no Property "{propertyName}".</exception>
    /// <exception cref="NullReferenceException">{obj}.{propertyName} is null.</exception>
    /// <returns>The value</returns>
    public static T GetPropertyValue<T>(object obj, string propertyName)
    {
        var property = obj.GetType().GetProperty(propertyName);
        if (property == null) throw new PropertyNotFoundException($"{obj} has no Property \"{propertyName}\".");

        var value = property.GetValue(obj, null);
        if (value == null)
            throw new NullReferenceException($"{obj}.{propertyName} is null.");

        return (T)value;
    }
}