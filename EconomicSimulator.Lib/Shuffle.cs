namespace EconomicSimulator.Lib;

public static class Shuffle
{
    public static IEnumerable<T> RandomShuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(a => Random.Shared.Next());
    }
}