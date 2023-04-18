using EconomicSimulator.Types;

public readonly record struct JobType(string Name,
    string Description,
    string TypeName,
    decimal WorkHoursNeeded,
    int MinWorkers,
    int MaxWorkers,
    IEnumerable<IOItem> Inputs,
    IEnumerable<IOItem> Outputs,
    IEnumerable<ToolType> Tools)
{
    public static implicit operator JobType(string name) => JobTypes.Get(name);

    public ItemRequirements GetRequirements()
    {
        return new(Inputs.Select(a => new ItemRequirement(a.Item, a.Count)));
    }
}