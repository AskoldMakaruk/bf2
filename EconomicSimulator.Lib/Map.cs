using System.Text;
using EconomicSimulator;
using EconomicSimulator.Interfaces;
using EconomicSimulator.Lib.Entities;
using EconomicSimulator.Types;
using Geolocation;


public record JobPost(Guid Guid, IFacility Facility, JobType Type, int Slots)
{
    public virtual bool Equals(JobPost? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Guid.Equals(other.Guid);
    }

    public override int GetHashCode()
    {
        return Guid.GetHashCode();
    }

    private int _slots = Slots;

    public JobPost(IFacility Facility, JobType Type, int Slots) : this(Guid.NewGuid(), Facility, Type, Slots)
    {
    }

    public int Slots
    {
        get => _slots;
        set => _slots = value;
    }
};

public class DeliveryNetwork
{
    private List<DeliveryRequirement> _deliveryRequirements = new();
    public IReadOnlyList<DeliveryRequirement> DeliveryRequirements => _deliveryRequirements.AsReadOnly();

    public List<Facility> Facilities { get; set; }
    public List<Worker> Workers { get; set; }

    public void PostDeliveryRequirement(DeliveryRequirement deliveryRequirement)
    {
        _deliveryRequirements.Add(deliveryRequirement);
    }
}

public class Map
{
    public List<Facility> Facilities { get; set; }
    public List<Worker> Workers { get; set; }
    private IEnumerable<JobPost> PreviousJobs = Array.Empty<JobPost>();
    private IEnumerable<DeliveryRequirement> _deliveryRequirements => Facilities.OrderBy(a => a.Workers.Count).SelectMany(a => a.GetDeliveryRequirements());

    public void ProcessWorkers()
    {
        foreach (var worker in Workers)
        {
            worker.ProcessNeeds();

            foreach (var requirement in worker.GetRequirements())
            {
                if (GetProducerNear(worker.Location, requirement) is not { } facilityNear) continue;

                ITrading me = worker;
                ITrading trader = facilityNear;
                if (me.TryBuyFrom(trader, requirement))
                {
                    // Console.WriteLine("happy purcashe");
                }
            }

            foreach (var r in _deliveryRequirements)
            {
                // if (!r.CanBeSatisfied(worker.Inventory)) continue;
                ITrading me = worker;
                // if (GetProducerNear(worker.Location, r) is not ITrading producer) continue;
                // if (!me.TryBuyFrom(producer, r)) continue;

                me.Prices["water"] = 9;
                ITrading buyer = r.Facility;
                if (buyer.TryBuyFrom(me, r))
                {
                    // Console.WriteLine("happy sell");
                }
            }

            worker.Consume();
            if (!worker.NeedsSomething())
            {
                worker.Status = WorkerStatus.Idle;
            }

            if (worker.Status == WorkerStatus.SeekingWork)
            {
                if (GetJobPost(new ItemRequirements(worker.GetRequirements())).FirstOrDefault(a => a.Facility.TryHire(worker, a)) is { } post)
                {
                    worker.Status = WorkerStatus.Working;
                }
            }

            if (worker.Status == WorkerStatus.SatisfyNeed)
            {
                worker.Status = WorkerStatus.SeekingWork;
            }
        }
    }

    private Facility? GetProducerNear(Location workerLocation, ItemRequirement itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetProducibleItems().Any(itemNeededItem.Matches));
    }

    private IEnumerable<JobPost> GetJobPost(ItemRequirements requirements)
    {
        return PreviousJobs.Where(a => a.Slots > 0).Where(a => requirements.CanBeSatisfied(new ManyItems(a.Type.Outputs)));
    }


    private Facility? GetProducerNear(Location workerLocation, ItemRequirements requirements)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.GetProducibleItems().Any(type => requirements.Requirements.Any(a => a.Matches(type))));
    }

    public void ProcessFacilities()
    {
        var posts = new LinkedList<JobPost>();
        foreach (var facility in Facilities)
        {
            facility.Process();
            foreach (var post in facility.GetJobPosts())
            {
                posts.AddLast(post);
            }
        }

        PreviousJobs = posts;
    }

    private Facility? GetProducerNear(Location workerLocation, ItemType itemNeededItem)
    {
        return GetFacilitiesNear(workerLocation).FirstOrDefault(a => a.CanProduce(itemNeededItem));
    }

    public string Report()
    {
        var report = new StringBuilder();
        report.AppendLine("Facilities:");
        foreach (var facility in Facilities)
        {
            report.AppendLine($"  {facility.Name} - {facility.Type} - {facility.Inventory.Report()}");
        }

        report.AppendLine("Workers:");
        foreach (var worker in Workers)
        {
            report.AppendLine($"  {worker.Name} - {worker.Status} - {worker.TotalExperience} - {worker.Balance} - {string.Join(",", worker.Needs.Select(a => a.Type.Name + " " + a.Progress.Value))}");
            report.AppendLine($"  Інвентар:");
            foreach (var item in worker.Inventory.Items)
            {
                report.AppendLine($"    {item.Key.Name} - {item.Value}");
            }
        }

        return report.ToString();
    }

    public List<Facility> GetFacilitiesNear(Location location)
    {
        return Facilities.Where(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters) < 5000).ToList();
    }


    public IEnumerable<ItemType> ProducedNear(Location location)
    {
        return GetFacilitiesNear(location).SelectMany(a => a.GetProducibleItems());
    }

    public Facility? GetClosestFacilityWithRequirement(Location location, ItemRequirement itemRequirement)
    {
        var facilities = GetFacilitiesNear(location);
        return facilities.OrderBy(a => GeoCalculator.GetDistance(a.Location, location, distanceUnit: DistanceUnit.Meters))
            .FirstOrDefault(a => itemRequirement.GetProposals(a.Inventory).Any());
    }
}