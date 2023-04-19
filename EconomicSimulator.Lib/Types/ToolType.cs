using EconomicSimulator;
using EconomicSimulator.Types;

public readonly record struct ToolType(ItemType Type, IEnumerable<IOItem> Recipe)
{
    public static implicit operator ToolType((string, IEnumerable<IOItem> ) tuple) => new ToolType(ItemTypes.Get(tuple.Item1), tuple.Item2);
    public static implicit operator ToolType(string name ) => ToolTypes.Get(name);
}