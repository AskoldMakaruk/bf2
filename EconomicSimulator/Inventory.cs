using EconomicSimulator.Types;

public class Inventory
{
    public Dictionary<ItemType, int> Items { get; set; } = new();

    public int this[ItemType type]
    {
        get => Items.TryGetValue(type, out var value) ? value : 0;
        set => Items[type] = value;
    }

    public string Report()
    {
        return Items.Aggregate("", (current, item) => current + $"{item.Key.Name}x{item.Value} ");
    }


    public bool HasItem(ItemType itemType)
    {
        return Items.Any(a => a.Key == itemType);
    }

    public bool TryRemoveItem(ItemType item, int count)
    {
        if (!HasItem(item)) return false;
        if (Items[item] < count) return false;
        Items[item] -= count;
        return true;
    }

    public void Add(IOItem item)
    {
        Items.TryAdd(item.Item, 0);
        Items[item.Item] += item.Count;
    }
}