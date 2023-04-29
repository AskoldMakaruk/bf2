using System.Text.Json.Serialization;
using EconomicSimulator.Lib.Entities;


namespace EconomicSimulator.Lib.Types;

public readonly record struct JobType(string Name,
    string Description,
    string TypeName,
    decimal WorkHoursNeeded,
    int MinWorkers,
    int MaxWorkers,
    [property: JsonIgnore] IEnumerable<IOItem> Inputs,
    [property: JsonIgnore] IEnumerable<IOItem> Outputs,
    [property: JsonIgnore] IEnumerable<ToolType> Tools)
{
    public static implicit operator JobType(string name) => JobTypes.Get(name);

    public ItemRequirements GetRequirements()
    {
        return new(Inputs.Select(a => new ItemRequirement(a.Item, a.Count)));
    }
}