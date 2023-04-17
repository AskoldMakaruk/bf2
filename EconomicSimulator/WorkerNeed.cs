public class WorkerNeed
{
    public WorkerNeedType Type { get; set; }
    public Progress Progress { get; set; }

    //todo move invertory to some interface
    public bool TryToSatisfy(Inventory inventory)
    {
        var result = false;

        while (IsNeeded() && Type.Requirements.All(c => c.CanBeFullfiled(inventory)))
        {
            result = true;
            // todo take all items at once
            foreach (var item in Type.Requirements.Where(item => !inventory.TryRemoveItem(item.Matches, item.Count)))
            {
                Console.WriteLine($"cannot remove item {item.Type}");
                continue;
            }

            Progress -= Type.ProgressPerItem;
        }

        return result;
    }

    public bool IsNeeded() => Progress.Value >= Type.ProgressPerItem;
}