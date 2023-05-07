using System.Numerics;

namespace EconomicSimulator.Lib;

public class Counter<TKey> : Dictionary<TKey, HumanHours> where TKey : notnull
{
    public Counter(IDictionary<TKey, HumanHours> input) : base(input)
    {
    }

    public Counter()
    {
    }

    public HumanHours? Get(TKey key)
    {
        if (TryGetValue(key, out var value))
        {
            return value;
        }

        return default;
    }


    public void AddForEach(HumanHours addable)
    {
        foreach (var key in Keys)
        {
            this[key] = (this[key]) + addable;
        }
    }

    public new void Add(TKey key, HumanHours value)
    {
        if (TryAdd(key, value))
        {
            return;
        }

        this[key] = value + this[key]!;
    }

    public new HumanHours? this[TKey key]
    {
        get => TryGetValue(key, out var value) ? value : default;
        set
        {
            if (value == null)
            {
                return;
            }

            if (ContainsKey(key))
            {
                base[key] = value.Value;
            }
            else
            {
                Add(key, value.Value);
            }
        }
    }
}