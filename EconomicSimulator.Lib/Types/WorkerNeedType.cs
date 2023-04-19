using EconomicSimulator;

public readonly record struct WorkerNeedType(string Name,
    string Description,
    string TypeName,
    decimal ProgressPerItem,
    ItemRequirements Requirements)
{
    public static implicit operator WorkerNeedType(string name) => WorkerNeedTypes.Get(name);
}