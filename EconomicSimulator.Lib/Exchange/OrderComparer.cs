namespace EconomicSimulator.Lib.Exchange;

public class OrderComparer : IComparer<Order>
{
    public int Compare(Order? x, Order? y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        var priceComparison = x.Price.Value.CompareTo(y.Price);
        if (priceComparison != 0) return priceComparison;
        return x.Added.CompareTo(y.Added);
    }
}