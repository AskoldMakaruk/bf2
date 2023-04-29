using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Lib.Types;

namespace EconomicSimulator.Lib.Interfaces;

public interface IManyItems : IEnumerable<IOItem>
{
    public IDictionary<ItemType, int> Items { get; }

    public IEnumerable<IOItem> GetItems()
    {
        return GetIoItems();
    }

    public IEnumerable<IOItem> GetIoItems() => Items.Select(a => new IOItem(a.Key, a.Value));
    public IEnumerable<ItemType> GetItemsTypes(int minCount = 0) => Items.Where(a => a.Value > minCount).Select(a => a.Key);

    IEnumerator<IOItem> IEnumerable<IOItem>.GetEnumerator()
    {
        return GetItems().GetEnumerator();
    }

    public int this[ItemType type]
    {
        get => Items.TryGetValue(type, out var value) ? value : 0;
        set => Items[type] = value;
    }
}