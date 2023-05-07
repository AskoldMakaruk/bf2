using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Networks.Exchange;

public class OrderMatch
{
    public decimal Price { get; set; }
    public ItemType ItemType { get; set; }
    public int Amount { get; set; }
    public string From { get; set; }
    public string To { get; set; }

    public override string ToString()
    {
        return $"{From} -> {To} {ItemType}x{Amount} {Price}HH";
    }
}