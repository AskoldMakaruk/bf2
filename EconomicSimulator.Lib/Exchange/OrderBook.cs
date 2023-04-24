namespace EconomicSimulator.Lib.Exchange;

public class OrderBook
{
    private SortedSet<Order> Bids { get; set; } = new(); // buyers
    private SortedSet<Order> Asks { get; set; } = new(); // sellers
    public Queue<OrderMatch> Matches { get; } = new();

    public void Add(Order order)
    {
        var (sameSet, compareTo) = order.Direction switch
        {
            OrderDirection.Buy => (Bids, Asks.Min),
            OrderDirection.Sell => (Asks, Bids.Max),
        };

        if (TryMatchTwo(order, compareTo) && order.IsFulfilled)
        {
            // instant satisfaction
        }
        else
        {
            sameSet.Add(order);
        }
    }

    private bool TryMatchTwo(Order? one, Order? two)
    {
        if (one == null || two == null || !two.SatisfiedByPrice(one.Price) || !one.SatisfiedByPrice(two.Price)) return false;

        var toSellAmount = Math.Min(one.Amount, two.Amount);
        one.AmountFulfilled += toSellAmount;
        two.AmountFulfilled += toSellAmount;

        var match = new OrderMatch()
        {
            Amount = toSellAmount,
            From = two.Author,
            To = one.Author,
            ItemType = one.ItemType,
            Price = (one.Price + two.Price) / 2
        };
        Matches.Enqueue(match);

        Console.WriteLine(match);


        if (two.IsFulfilled)
        {
            Asks.Remove(two);
        }

        if (one.IsFulfilled)
        {
            Bids.Remove(one);
        }

        return true;
    }

    public void MatchOrders()
    {
        while (TryMatchTwo(Bids.Max, Asks.Min))
        {
        }
    }
}