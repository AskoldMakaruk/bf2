namespace EconomicSimulator.Lib;

public class StartStatic
{
    public static void Start()
    {
        var well1 = new Facility()
        {
            Id = Guid.NewGuid(),
            Inventory = new Inventory(),
            Location = new Location(1, 1),
            Name = "Water Well 1",
            Type = "water_well",
            Prices = new List<SellingPrice>()
            {
                ("water", 8)
            },
            JobTypes =
            {
                "collect_water_w_bucket",
            }
        };

        var farm1 = new Facility()
        {
            Id = Guid.NewGuid(),
            Inventory = new Inventory(),
            Location = new Location(1, 1.0001),
            Name = "Farm 1",
            Type = "ground_farm",
            Prices = new List<SellingPrice>()
            {
                ("wheat", 40),
                ("water", 9)
            },
            JobTypes =
            {
                JobTypes.GrowWheat
            },
            Workers = new List<Worker>(),
            Balance = 1000m,
        };

        var facilities = new[]
        {
            well1,
            farm1
        };

        var map = new Map()
        {
            Facilities = facilities.ToList(),
            Workers = new[]
                {
                    "KOK", "balls", "Sir de La CUm", "Cumbotron 4000", "la fishe un chocolate", "hui", "pisun",
                    "joker", "totoro", "kupalnik", "kokroach", "oposum", "openheimer", "chad", "broski", "lemon",
                    "dick", "penis"
                }.Select(CreateWorker)
                .ToList()
        };

        while (true)
        {
            map.ProcessWorkers();
            map.ProcessFacilities();
            // Console.WriteLine(map.Report());
            Game.Time.Tick();
            Console.WriteLine(Game.Time.Display());
            Thread.Sleep(10);
            GameStats.PostFrames().Wait();
        }

        Worker CreateWorker(string name)
        {
            return new Worker()
            {
                Id = Guid.NewGuid(),
                Location = new Location(1.001, 1),
                Name = name,
                Inventory = new Inventory(),
                Needs = new List<WorkerNeed>()
                {
                    "thirst", "hunger"
                },
                TotalExperience = 1,
                Balance = 0
            };
        }
    }
}