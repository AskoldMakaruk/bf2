using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Entities;

public class Item
{
    public Guid Id { get; set; }
    public ItemType Type { get; set; }
}