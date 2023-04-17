namespace EconomicSimulator;

public static class WorkerNeedTypes
{
    private static readonly IReadOnlyCollection<WorkerNeedType> _facilityTypes = new[]
    {
        new WorkerNeedType("Пити водички", "хочеться питки", "thirst", ProgressPerItem: 0.2, new List<ItemRequirement>() { ("water", 1) }),
        new WorkerNeedType("Їсти", "хочеться їсти", "hunger", ProgressPerItem: 0.2, new List<ItemRequirement>() { ("meat", 1) }),
    };

    public static WorkerNeedType Get(string name)
    {
        return _facilityTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}