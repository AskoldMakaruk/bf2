using EconomicSimulator;
using EconomicSimulator.Types;

public readonly record struct IOItem(ItemType Item, int Count)
{
    public static implicit operator IOItem((string, int ) tuple) => new IOItem(ItemTypes.Get(tuple.Item1), tuple.Item2);
    public static implicit operator IOItem(KeyValuePair<ItemType, int> tuple) => new IOItem(tuple.Key, tuple.Value);

    public decimal GetPrice(SellingPrice price) => Count * price.Price;
}