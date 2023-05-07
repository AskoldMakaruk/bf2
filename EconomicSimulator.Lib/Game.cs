using System.Text;
using EconomicSimulator.Lib.Entities;

namespace EconomicSimulator.Lib;

public static class Game
{
    public static readonly TimeSimulator Time = new TimeSimulator();

    public static List<Facility> Facilities { get; set; }
    public static List<Worker> Workers { get; set; }


    public static void ProcessWorkers()
    {
        foreach (var worker in Workers)
        {
            worker.Process();
        }
    }

    public static void ProcessFacilities()
    {
        foreach (var facility in Facilities)
        {
            facility.Process();
        }
    }

    public static string Report()
    {
        var report = new StringBuilder();
        report.AppendLine("Facilities:");
        foreach (var facility in Game.Facilities)
        {
            report.AppendLine($"  {facility.Name} - {facility.Type} - {facility.Inventory.Report()}");
        }

        report.AppendLine("Workers:");
        foreach (var worker in Game.Workers)
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

    public static void Process()
    {
        Game.ProcessWorkers();
        Game.ProcessFacilities();
        Game.Time.Tick();
    }
}