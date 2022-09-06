namespace GenericBlazorWebApi.Tests.Shared.TestModels;

public class TestModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Nullable { get; set; } = string.Empty;
    public string UniqueName { get; set; } = string.Empty;
}