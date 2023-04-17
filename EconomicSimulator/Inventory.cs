using EconomicSimulator.Types;

public class Inventory
{
    public Dictionary<ItemType, int> Items { get; set; } = new();

    public IEnumerable<IOItem> GetIoItems() => Items.Select(a => new IOItem(a.Key, a.Value));
    public IEnumerable<ItemType> GetItemsTypes(int minCount = 0) => Items.Where(a => a.Value > minCount).Select(a => a.Key);

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

    public bool TryRemoveItem(Func<ItemType, bool> predicate, int count)
    {
        foreach (var item in Items.Where(a => predicate(a.Key)))
        {
            if (item.Value < count)
            {
                count -= item.Value;
                Items[item.Key] = 0;
            }
            else
            {
                Items[item.Key] -= count;
                return true;
            }

            if (count == 0)
            {
                return true;
            }
        }

        return false;
    }

    public bool TryRemoveItem(IOItem ioitem)
    {
        var (item, count) = ioitem;
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