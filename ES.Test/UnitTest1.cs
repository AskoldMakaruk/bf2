using EconomicSimulator.Lib.Exchange;
using FluentAssertions;

namespace ES.Test;

public class Tests
{
    private OrderBook _orderBook;

    [SetUp]
    public void Setup()
    {
        _orderBook = new OrderBook();
    }

    [Test]
    public void Test1()
    {
        var asks =
            new Order()
            {
                Price = 10,
                ItemType = "water",
                Amount = 10,
                Author = "Seller Sellingston",
                Direction = OrderDirection.Sell
            };

        var bid =
            new Order()
            {
                Price = 11,
                ItemType = "water",
                Amount = 3,
                Direction = OrderDirection.Buy,
                Author = "Buyer Buyingston",
            };

        _orderBook.Add(asks);
        _orderBook.Add(bid);

        _orderBook.MatchOrders();
        _orderBook.Matches.Should().NotHaveCount(0);
    }
}