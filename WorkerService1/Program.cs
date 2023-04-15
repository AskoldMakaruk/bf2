using Autofac;
using Autofac.Extensions.DependencyInjection;
using BF2;

var builder = Host.CreateDefaultBuilder(args);
builder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(container => { })
    .ConfigureServices(services => { services.UseBF2(provider => provider.GetService<IConfiguration>()!["BotToken"]!); });

var app = builder.Build();
app.Run();

// content + buttons = view

public class StartController : BotController
{
    [Command("/start")]
    public async Task<View> Start()
    {
        return new StartView(Update.Message.From.Username);
    }

    [Button("Help")]
    public async Task<View> Help()
    {
        return "new string view";
    }
}


public class HelpView : View
{
    public override string ToView()
    {
        return "123";
    }
}


public class StartView : View
{
    private readonly string _userName;

    public StartView(string userName)
    {
        _userName = userName;
    }

    public override string ToView()
    {
        return $"Hi, {_userName}";
    }
}