using System.Collections.Concurrent;

namespace EconomicSimulator;

public static class GameStats
{
    private static readonly Dictionary<string, int> Frames = new();

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
        }

        Frames.Clear();
    }


    public static readonly ConcurrentQueue<StatFrame> Stats = new();
}

public readonly record struct StatFrame(string Name, int Count);