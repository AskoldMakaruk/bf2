using BF2;
using EconomicSimulator.Lib;
using EconomicSimulator.Lib.Types;
using Neo4j.Driver;
using Telegram.Bot;

// var builder = Host.CreateDefaultBuilder(args);
// builder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
//     .ConfigureContainer<ContainerBuilder>(container => { })
//     .ConfigureServices(services => { services.UseBF2(provider => provider.GetService<IConfiguration>()!["BotToken"]!); });
//
// var app = builder.Build();
// app.Run();
var uri = "neo4j+s://4f7f6eee.databases.neo4j.io";
var user = "neo4j";
var password = "A5gm7hDRETtZIJSwjqjV55EgZAT1EZKDD2HUJyc03nE";
var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(user, password));
await driver.VerifyConnectivityAsync();
var client = new Neo4jClient.BoltGraphClient(driver);

await client.ConnectAsync();
foreach (var itemType in ItemTypes._ItemTypes)
{
    await client.Cypher.Write
        .Merge("(n: Item  {TypeName: $typeName})")
        .OnCreate()
        .Set("n = $item")
        .WithParams(new
        {
            typeName = itemType.TypeName,
            item = itemType
        })
        .ExecuteWithoutResultsAsync();
}

foreach (var jobType in JobTypes._jobTypes)
{
    var jobInput = new
    {
        jobType.Name, jobType.Description,
        jobType.TypeName,
        jobType.WorkHoursNeeded,
        jobType.MinWorkers,
        jobType.MaxWorkers,
    };
    await client.Cypher.Write
        .Merge("(n: Job {TypeName: $typeName})")
        .OnCreate()
        .Set("n = $job")
        .WithParams(new
        {
            typeName = jobType.TypeName,
            job = jobInput
        })
        .ExecuteWithoutResultsAsync();

    foreach (var output in jobType.Outputs)
    {
        await client.Cypher.Write
            .Match("(type:Item {TypeName: $itemType})",
                "(job:Job {TypeName: $jobType})")
            .Merge("(job)-[:OUTPUT]->(type)")
            .WithParams(new
            {
                itemType = output.Item.TypeName,
                jobType = jobType.TypeName
            })
            .ExecuteWithoutResultsAsync();
    }
}

foreach (var facilityType in FacilityTypes._facilityTypes)
{
    var type = new
    {
        facilityType.Name,
        facilityType.Description,
        facilityType.TypeName,
    };
    await client.Cypher.Write
        .Merge("(n: Facility {TypeName: $typeName})")
        .OnCreate()
        .Set("n = $facility")
        .WithParams(new
        {
            typeName = facilityType.TypeName,
            facility = type
        })
        .ExecuteWithoutResultsAsync();

    foreach (var job in facilityType.Jobs)
    {
        await client.Cypher.Write
            .Match("(fac:Facility {TypeName: $facType})",
                "(job:Job {TypeName: $jobType})")
            .Merge("(fac)-[:HAS_JOB]->(job)")
            .WithParams(new
            {
                jobType = job.TypeName,
                facType = facilityType.TypeName
            })
            .ExecuteWithoutResultsAsync();
    }
}
// StartStatic.Start();

// content + buttons = view
public class StartController : BotController
{
    [Command("/start")]
    public async Task<View> Start(TelegramBotClient client)
    {
        return null;
        // return new StartView(Update.Message.From.Username);
    }
}

public class StartView : View
{
    public override string Format => "";
}