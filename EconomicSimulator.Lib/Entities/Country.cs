using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

public class Country
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<ItemType, HumanHours> Wages { get; private set; } = new();
}