using System.Collections;
using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Interfaces;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Properties;

public class ManyItems : IManyItems
{
    public IDictionary<ItemType, int> Items { get; }

    public IEnumerator GetEnumerator()
    {
        return (this as IManyItems).GetEnumerator();
    }

    public ManyItems(IEnumerable<IOItem> items)
    {
        Items = items.ToDictionary(a => a.Item, a => a.Count);
    }

    public static implicit operator ManyItems(List<IOItem> items) => new(items);
}

public class Inventory : IManyItems
{
    public IDictionary<ItemType, int> Items { get; set; } = new Dictionary<ItemType, int>();

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

    // todo fix partial removals
    public bool TryRemoveItems(IEnumerable<IOItem> ioitem)
    {
        var result = ioitem.Aggregate(true, (current, item) => current & TryRemoveItem(item));

        return result;
    }

    public void AddRange(IEnumerable<IOItem> items)
    {
        foreach (var ioItem in items)
        {
            Add(ioItem);
        }
    }

    public void Add(IOItem item)
    {
        Items.TryAdd(item.Item, 0);
        Items[item.Item] += item.Count;
    }


    public IEnumerator GetEnumerator()
    {
        return (this as IManyItems).GetItems().GetEnumerator();
    }
}