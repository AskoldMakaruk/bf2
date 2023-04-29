using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Interfaces;

namespace EconomicSimulator.Lib.Types;

/// <summary>
/// Contains data for item requirement
/// </summary>
public class ItemRequirement
{
    public ItemRequirement(ItemType type, int count, decimal? price)
    {
        Type = type;
        Count = count;
        Price = price;
    }

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
    public decimal? Price { get; }

    public IEnumerable<ItemType> Matches(IEnumerable<ItemType> itemTypes)
    {
        if (HashTag is { } tag)
        {
            var hashTagTypes = tag.GetHashtagItems().ToList();
            foreach (var itemType in itemTypes.Where(itemType => hashTagTypes.Contains(itemType)))
            {
                yield return itemType;
            }

            yield break;
        }

        if (Type is { } type && itemTypes.Contains(type))
        {
            yield return type;
        }
    }

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

    public IEnumerable<FulfilmentVariant> GetProposals(IManyItems inventory)
    {
        var items = inventory.GetItemsTypes(Count).ToList();
        var matches = Matches(items).ToList();
        if (matches.Any())
        {
            yield return new FulfilmentVariant(this, matches.Select(a => new IOItem(a, Math.Min(Count, inventory[a]))).ToList());
        }
    }
}


public readonly record struct FulfilmentVariant(ItemRequirement Requirement, List<IOItem> Proposal)
{
    public decimal GetCost(SellingPrice price)
    {
        return Proposal.Sum(a => a.GetPrice(price.Price));
    }
}