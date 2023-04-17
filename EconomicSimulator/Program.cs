// See https://aka.ms/new-console-template for more information


using System.Collections;
using EconomicSimulator;
using EconomicSimulator.Types;
using Geolocation;

var worker = new Worker()
{
    Id = Guid.NewGuid(),
    Location = new Location(1.001, 1),
    Name = "Kok",
    Inventory = new Inventory(),
    Needs = new List<WorkerNeed>()
    {
        new WorkerNeed()
        {
            Progress = new Progress(0),
            Type = "thirst"
        }
    },
    TotalExperience = 1,
    WorkHours = 0
};

var well1 = new Facility()
{
    Id = new Guid("BDD1A533-546E-4677-B718-2711D4ED95EE"),
    Inventory = new Inventory(),
    Location = new Location(1, 1),
    Name = "Factory1",
    Type = "water_well",
    Prices = new List<SellingPrice>()
    {
        ("water", 10)
    },
    JobQueue =
    {
        new Job()
        {
            Id = Guid.NewGuid(),
            Type = "collect_water_w_bucket",
            CurrentProgress = 0
        }
    }
};

var facilities = new[]
{
    well1
};

var map = new Map()
{
    Facilities = facilities.ToList(),
    Workers = new List<Worker>() { worker }
};

while (true)
{
    map.ProcessWorkers();
    map.ProcessFacilities();
    Console.WriteLine(map.Report());
    Console.ReadLine();
}


public class Location
{
    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public static implicit operator Coordinate(Location location) => new Coordinate(location.Latitude, location.Longitude);
}

public enum WorkerStatus
{
    Idle,
    Working,
    SeekingWork,
    SatisfyNeed
}

public record Progress(double Value)
{
    public double Value { get; set; } = Value;
    public static implicit operator Progress(double value) => new Progress(value);
    public static implicit operator double(Progress value) => value.Value;
}

public readonly record struct WorkHours(double Value)
{
    public static implicit operator WorkHours(double value) => new WorkHours(value);
    public static implicit operator double(WorkHours value) => value.Value;
}

public record SellingPrice(ItemType Item, WorkHours Price)
{
    public static implicit operator SellingPrice((string, double) tuple) => new(ItemTypes.Get(tuple.Item1), new WorkHours(tuple.Item2));
}

public readonly record struct IOItem(ItemType Item, int Count)
{
    public static implicit operator IOItem((string, int ) tuple) => new IOItem(ItemTypes.Get(tuple.Item1), tuple.Item2);
    public static implicit operator IOItem(KeyValuePair<ItemType, int> tuple) => new IOItem(tuple.Key, tuple.Value);
}

public class Item
{
    public Guid Id { get; set; }
    public ItemType Type { get; set; }
}


public class Tool : Item
{
}

public class Job
{
    public Guid Id { get; set; }
    public JobType Type { get; set; }
    public WorkHours CurrentProgress { get; set; }
}