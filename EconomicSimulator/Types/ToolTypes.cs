public static class ToolTypes
{
    public static readonly ToolType water_bucker = ("water_bucket", new List<IOItem>());
    public static readonly ToolType wheal_saw = ("wheal_saw", new List<IOItem>());

    private static readonly IReadOnlyCollection<ToolType> _ToolTypes = new[]
    {
        water_bucker
    };

    public static ToolType Get(string name)
    {
        return _ToolTypes.FirstOrDefault(x => string.Equals(x.Type.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}