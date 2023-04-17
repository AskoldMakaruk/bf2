using EconomicSimulator;

public readonly record struct WorkerNeedType(string Name,
    string Description,
    string TypeName,
    double ProgressPerItem,
    List<ItemRequirement> Requirements)
{
    public static implicit operator WorkerNeedType(string name) => WorkerNeedTypes.Get(name);
}