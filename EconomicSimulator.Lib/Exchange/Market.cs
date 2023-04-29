using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Exchange;

public static class Market
{
    public static Dictionary<ItemType, OrderBook> Books = new();

    public static void AddOrder(Order order)
    {
        Books.TryAdd(order.ItemType.TypeName, new OrderBook());

        var book = Books[order.ItemType.TypeName];
        book.Add(order);
    }

    public static void AddDelivery(IEnumerable<DeliveryRequirement> deliveryRequirements)
    {
        foreach (var delivery in deliveryRequirements)
        {
            foreach (var requirement in delivery.Requirements)
            {
                var types = MatchesRequirement(requirement).ToList();
                if (types.Count == 0) continue;

                AddOrder(new Order
                {
                    ItemType = types.First(),
                    Added = DateTime.Now,
                    Amount = requirement.Count,
                    Author = delivery.Facility.Name,
                    Direction = OrderDirection.Buy,
                    Price = requirement.Price
                });
            }
        }
    }

    public static bool TryBuyForMarketPrice()
    {
        return false;
    }

    public static IEnumerable<ItemType> MatchesRequirement(ItemRequirement requirement)
    {
        return requirement.Matches(Books.Select(a => a.Key));
    }
}