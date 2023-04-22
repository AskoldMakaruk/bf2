using EconomicSimulator.Types;

namespace EconomicSimulator.Lib.Entities;

public interface IFacility
{
    public IEnumerable<Worker> GetFreeWorkers();
    public bool TryHire(Worker worker, JobPost post);
    public List<Worker> Workers { get; }
    public FacilityType Type { get; }
    public Inventory Inventory { get; }
    public HumanHours? GetPrice(ItemType itemType);

    public void Income(HumanHours humanHours);
    public void SubmitProgress(JobResult result);
}