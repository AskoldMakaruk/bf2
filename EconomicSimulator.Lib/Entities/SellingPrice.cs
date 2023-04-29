using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

public record SellingPrice(ItemType Item, HumanHours Price)
{
    public static implicit operator SellingPrice((string, decimal) tuple) => new(ItemTypes.Get(tuple.Item1), new HumanHours(tuple.Item2));
}