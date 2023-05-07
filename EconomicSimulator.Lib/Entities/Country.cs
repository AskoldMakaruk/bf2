using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

public class Country
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<ItemType, HumanHours> Wages { get; private set; } = new();

    public Country(string name, string code)
    {
        Name = name;
        Code = code;
    }
}