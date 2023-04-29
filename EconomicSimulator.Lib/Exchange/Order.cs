using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Exchange;

public class Order
{
    public OrderDirection Direction { get; set; }
    public decimal? Price { get; set; }
    public int Amount { get; set; }
    public int AmountFulfilled { get; set; }
    public ItemType ItemType { get; set; }
    public DateTime Added { get; set; }
    public string Author { get; set; }

    public bool SatisfiedByPrice(decimal? price)
    {
        if (Price == null)
        {
            return true;
        }
        return Direction == OrderDirection.Buy && price <= Price || Direction == OrderDirection.Sell && price >= Price;
    }

    public bool IsFulfilled => AmountFulfilled >= Amount;
    public int AmountLeft => Amount - AmountFulfilled;
    public bool IsMarketOrder => Price == null;
}