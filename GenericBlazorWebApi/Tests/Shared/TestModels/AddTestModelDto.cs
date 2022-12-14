namespace GenericBlazorWebApi.Tests.Shared.TestModels;

public class AddTestModelDto
{
    public string Name { get; set; } = string.Empty;

    public string? Nullable { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
}