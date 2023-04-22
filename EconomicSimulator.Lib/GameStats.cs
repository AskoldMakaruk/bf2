using System.Collections.Concurrent;
using EconomicSimulator.Lib;
using ScottPlot;
using SkiaSharp;

namespace EconomicSimulator;

public static class GameStats
{
    private static readonly Dictionary<string, List<int>> History = new();
    private static readonly Dictionary<string, int> Frames = new();

    public static void SavePng()
    {
        Plot myPlot = new();
        foreach (var history in History)
        {
            var coordinates = history.Value.Chunk(1000).TakeLast(60).Select((a, t) => new Coordinates(t * 1000 + a.Length, a.Sum() / (double)a.Length)).ToList();
            var sig1 = myPlot.Add.Scatter(coordinates);
            sig1.Label = history.Key;
        }


        Directory.CreateDirectory("production");
        myPlot.SavePng($"production/ProductionStats_{Game.Time.Ticks}.png", 300, 300);
        Console.WriteLine($"Saved tick {Game.Time.Ticks}");
    }

    // Adds new frame to stats
    public static void Post(string name, int count)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");
        }

        if (Frames.TryGetValue(name, out int currentCount))
        {
            Frames[name] = currentCount + count;
        }
        else
        {
            Frames[name] = count;
        }
    }

    // send all stat frames to socket stream
    public static async Task PostFrames()
    {
        var frames = Frames.Select(stat => new StatFrame(stat.Key, stat.Value)).ToList();
        foreach (var frame in frames)
        {
            Stats.Enqueue(frame);
            if (!History.TryAdd(frame.Name, new List<int>() { frame.Count }))
            {
                History[frame.Name].Add(frame.Count);
            }
        }

        Frames.Clear();
    }


    public static readonly ConcurrentQueue<StatFrame> Stats = new();
}

public readonly record struct StatFrame(string Name, int Count);