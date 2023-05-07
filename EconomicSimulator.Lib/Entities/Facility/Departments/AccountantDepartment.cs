namespace EconomicSimulator.Lib.Entities;

/// <summary>
/// Відповідає за бюджет і виплати
/// </summary>
public class AccountantDepartment : Department
{
    public AccountantDepartment(IFacility facility) : base(facility)
    {
    }

    public HumanHours Balance { get; set; }

    public void ProcessPayouts(JobResult result)
    {
        // todo 
        // take worker hours
        // for 80% of it buy facility product
        // pay rest as HH
        foreach (var (worker, hours) in result.ProgressMade)
        {
            var rate = Facility.Type.Payouts;

            decimal b = Balance;
            var productionPercentage = b switch
            {
                < 400m => 1.0m,
                < 1000m => 1.0m - rate.MaxSubstitution / 2m,
                _ => 1.0m - rate.MaxSubstitution
            };

            var hh = 0m;
            var items = new List<IOItem>();
            foreach (var a in result.Products)
            {
                var countProducedByWorker = (int)Math.Floor((decimal)a.Count / result.ProgressMade.Count);
                var itemPrice = Facility.GetPrice(a.Item)!.Value;
                var price = itemPrice * countProducedByWorker;
                var count = (int)Math.Floor(countProducedByWorker * productionPercentage);

                var diff = countProducedByWorker - count;
                var priceDiff = price - itemPrice * diff;
                hh += priceDiff;

                items.Add(a with
                {
                    Count = count,
                });
            }

            var itemsToGive = Facility.Inventory.TryRemoveItems(items) ? items : new();
            var hhToGive = TrySpendMoney(hh) ? hh : 0;

            var payout = new Payout(hhToGive, itemsToGive);

            worker.ReceivePayout(payout);
        }
    }


    private bool TrySpendMoney(HumanHours humanHours)
    {
        if (humanHours.Value > Balance)
        {
            return false;
        }

        Balance -= humanHours.Value;
        return true;
    }
}

public record Payout(HumanHours Hours, List<IOItem> IoItems);