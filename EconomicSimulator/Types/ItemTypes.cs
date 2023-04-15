using EconomicSimulator.Types;

namespace EconomicSimulator;

public static class ItemTypes
{
    private static readonly IReadOnlyCollection<ItemType> _ItemTypes = new[]
    {
        new ItemType("Вода", "рідина", "water"),
        new ItemType("Відро", "може носити рідини", "bucket")
    };

    public static ItemType Get(string name)
    {
        return _ItemTypes.FirstOrDefault(x => string.Equals(x.TypeName, name, StringComparison.CurrentCultureIgnoreCase))!;
    }
}