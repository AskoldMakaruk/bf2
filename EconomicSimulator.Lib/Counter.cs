using System.Numerics;

namespace EconomicSimulator.Lib;

public class Counter<TKey, TAddable> : Dictionary<TKey, TAddable> where TAddable : IAdditionOperators<TAddable, TAddable, TAddable> where TKey : notnull
{
    public Counter(IDictionary<TKey, TAddable> input) : base(input)
    {
    }

    public Counter()
    {
    }

    public TAddable? Get(TKey key)
    {
        if (TryGetValue(key, out var value))
        {
            return value;
        }

        return default;
    }


    public void AddForEach(TAddable addable)
    {
        foreach (var key in Keys)
        {
            this[key] = (this[key]) + addable;
        }
    }

    public void Add(TKey key, TAddable value)
    {
        if (TryAdd(key, value))
        {
            return;
        }

        this[key] = value + this[key]!;
    }

    public new TAddable? this[TKey key]
    {
        get => TryGetValue(key, out var value) ? value : default;
        set
        {
            if (ContainsKey(key))
            {
                base[key] = value;
            }
            else
            {
                Add(key, value);
            }
        }
    }
}