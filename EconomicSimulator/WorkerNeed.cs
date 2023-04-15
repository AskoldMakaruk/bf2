public class WorkerNeed
{
    public WorkerNeedType Type { get; set; }
    public Progress Progress { get; set; }

    //todo move invertory to some interface
    public bool TryToSatisfy(Inventory inventory)
    {
        var result = false;

        while (Progress - Type.ProgressPerItem > 0 && CanSatisfy(inventory))
        {
            result = true;
            foreach (var item in Type.Items.Where(item => !inventory.TryRemoveItem(item.Item, item.Count)))
            {
                Console.WriteLine($"cannot remove item {item.Item}");
                continue;
            }

            Progress -= Type.ProgressPerItem;
        }

        return result;
    }

    public bool IsNeeded() => Progress.Value >= Type.ProgressPerItem;

    private bool CanSatisfy(Inventory inventory)
    {
        return Type.Items.All(a => inventory[a.Item] > a.Count);
    }
}