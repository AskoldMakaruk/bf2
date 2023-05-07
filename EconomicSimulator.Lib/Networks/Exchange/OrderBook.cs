namespace EconomicSimulator.Lib.Networks.Exchange;

public class OrderBook
{
    private SortedSet<Order> Bids { get; set; } = new(new OrderComparer()); // buyers
    private SortedSet<Order> Asks { get; set; } = new(new OrderComparer()); // sellers
    public Queue<OrderMatch> Matches { get; } = new();

    public void Add(Order order)
    {
        if (order.Price == null)
        {
            TryMarketOrder(order);
            return;
        }
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

    public bool TryMarketOrder(Order order)
    {
        var amountAcc = 0;
        var oppositeSet = order.Direction switch
        {
            OrderDirection.Buy => Asks.TakeWhile(order1 =>
                {
                    amountAcc += order1.AmountLeft;
                    return amountAcc < order.Amount;
                })
                .ToList(),
            OrderDirection.Sell => Bids.TakeWhile(order1 =>
                {
                    amountAcc += order1.AmountLeft;
                    return amountAcc < order.Amount;
                })
                .ToList(),
        };

        var result = false;
        foreach (var oppositeOrder in oppositeSet)
        {
            result |= TryMatchTwo(oppositeOrder, order);
        }

        return result;
    }

    private decimal? PriceBetweenTwo(Order one, Order orderTwo)
    {
        if (one.Price == null && orderTwo.Price == null) return null;
        if (one.Price == null) return orderTwo.Price;
        if (orderTwo.Price == null) return one.Price;
        return (one.Price + orderTwo.Price) / 2;
    }

    private bool TryMatchTwo(Order? one, Order? two)
    {
        if (one == null || two == null)
        {
            return false;
        }

        if (!two.SatisfiedByPrice(one.Price) || !one.SatisfiedByPrice(two.Price)) return false;

        var toSellAmount = Math.Min(one.Amount, two.Amount);
        one.AmountFulfilled += toSellAmount;
        two.AmountFulfilled += toSellAmount;
        var (seller, buyer) = (one.Direction, two.Direction) switch
        {
            (OrderDirection.Buy, OrderDirection.Sell) => (two, one),
            (OrderDirection.Sell, OrderDirection.Buy) => (one, two),
            _ => throw new Exception("Impossible")
        };

        var match = new OrderMatch()
        {
            Amount = toSellAmount,
            From = seller.Author,
            To = buyer.Author,
            ItemType = one.ItemType,
            Price = PriceBetweenTwo(one, two).Value,
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

    public void Print()
    {
        Console.WriteLine("Bids:");
        foreach (var bid in Bids.Take(5))
        {
            Console.WriteLine(bid);
        }

        Console.WriteLine("Asks:");
        foreach (var ask in Asks)
        {
            Console.WriteLine(ask);
        }
    }
}