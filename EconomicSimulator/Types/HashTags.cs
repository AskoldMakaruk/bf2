﻿using EconomicSimulator;
using EconomicSimulator.Types;

public static class HashTags
{
    public static readonly HashTag Food = new("#food");

    public static readonly IReadOnlyDictionary<HashTag, IEnumerable<ItemType>> HashtagItems = new Dictionary<HashTag, IEnumerable<ItemType>>()
    {
        { Food, new List<ItemType> { ItemTypes.Fruits } },
    };

    public static IEnumerable<ItemType> GetHashtagItems(this HashTag tag)
    {
        return HashtagItems[tag];
    }
}