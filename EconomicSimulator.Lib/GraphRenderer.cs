using EconomicSimulator.Lib;
using EconomicSimulator.Lib.Exchange;
using ScottPlot;

namespace EconomicSimulator;

public class OrderBookGraphDto
{
}

public static class GraphRenderer
{
    public static void OrderBookAsPng(SortedSet<Order> bids, SortedSet<Order> asks)
    {
        ScottPlot.Plot myPlot = new();
        // render Order Book - Depth Chart 

        // bids.

        // myPlot.SavePng($"production/OrderBool_{Game.Time.Ticks}.png", 300, 300);
    }

    public static void SavePng(Dictionary<string, List<int>> historyList)
    {
        Plot myPlot = new();
        foreach (var history in historyList)
        {
            var coordinates = history.Value.Chunk(1000).TakeLast(60).Select((a, t) => new Coordinates(t * 1000 + a.Length, a.Sum() / (double)a.Length)).ToList();
            var sig1 = myPlot.Add.Scatter(coordinates);
            sig1.Label = history.Key;
        }

        Directory.CreateDirectory("production");
        myPlot.SavePng($"production/ProductionStats_{Game.Time.Ticks}.png", 300, 300);
        Console.WriteLine($"Saved tick {Game.Time.Ticks}");
    }
}