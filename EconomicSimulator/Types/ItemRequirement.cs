using EconomicSimulator;
using EconomicSimulator.Types;

/// <summary>
/// Contains data for item requirement
/// </summary>
public class ItemRequirement
{
    public ItemRequirement(ItemType type, int count)
    {
        Type = type;
        Count = count;
    }

    public ItemRequirement(HashTag tag, int count)
    {
        HashTag = tag;
        Count = count;
    }

    public static implicit operator ItemRequirement((string, int ) tuple) => new ItemRequirement(ItemTypes.Get(tuple.Item1), tuple.Item2);
    public static implicit operator ItemRequirement((HashTag, int ) tuple) => new ItemRequirement(tuple.Item1, tuple.Item2);
    public HashTag? HashTag { get; }
    public ItemType? Type { get; }
    public int Count { get; }

    public bool Matches(ItemType itemType)
    {
        if (HashTag is { } tag)
        {
            var hashTagTypes = tag.GetHashtagItems();
            return hashTagTypes.Contains(itemType);
        }

        if (Type is { } type)
        {
            return type == itemType;
        }

        return false;
    }

    public bool CanBeFullfiled(Inventory inventory)
    {
        if (HashTag is { } tag)
        {
            var hashTagTypes = tag.GetHashtagItems();
            return inventory.Items.Where(a => hashTagTypes.Contains(a.Key)).Any(a => a.Value >= Count);
        }

        if (Type is { } type)
        {
            return inventory[type] >= Count;
        }

        return false;
    }
}