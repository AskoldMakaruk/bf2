using Autofac;
using Autofac.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WorkerService1;

var builder = Host.CreateDefaultBuilder(args)
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(container => { })
    .ConfigureServices(services =>
    {
        var controllers = TypeHelper.GetControllerTypes();
        foreach (var controller in controllers)
        {
            services.AddScoped(typeof(ViewController), controller);
            services.AddScoped(controller);
        }

        services.AddSingleton<TelegramBotClient>(provider => new(provider.GetService<IConfiguration>()!["BotToken"]!));
        services.AddSingleton<EndpointCollection>(provider =>
        {
            var controllers = provider.GetServices<ViewController>();
            var endpoints = controllers.SelectMany(controller => controller.GetEndpoints());
            return new EndpointCollection(endpoints);
        });

        services.AddHostedService<HostedBot>();
    });

var app = builder.Build();
app.Run();

// content + buttons = view

public class StartController : ViewController
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

public class StringView : View
{
    public string Text;

    public override string ToView()
    {
        return Text;
    }

    public static implicit operator StringView(string text) => new() { Text = text };
}

public class HelpView : View
{
    public override string ToView()
    {
        return "123";
    }
}

public abstract class ViewController
{
    public Update Update;
    public TelegramBotClient Bot;

    public virtual Task StartAsync()
    {
        return Task.CompletedTask;
    }


    public ReplyMarkupBase ToKeys()
    {
        var methods = this.GetMethods();
        var keys = methods.Select(a => a.GetCustomAttributes(typeof(ButtonAttribute), true).FirstOrDefault())
            .Where(a => a is ButtonAttribute)
            .Cast<ButtonAttribute>()
            .Select(a => new KeyboardButton(a.Text));

        return new ReplyKeyboardMarkup(keys.Chunk(2).ToList()) { ResizeKeyboard = true };
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

public abstract class View
{
    public abstract string ToView();
    public static implicit operator View(string text) => new StringView() { Text = text };
}

public abstract class Content
{
    public abstract Task SendAsync(ITelegramBotClient botClient, long chatId);
}

public class ImageContent : Content, IDisposable
{
    public void Dispose()
    {
        _stream.Dispose();
    }

    private readonly Stream _stream;

    public ImageContent(Stream stream)
    {
        _stream = stream;
    }

    public override async Task SendAsync(ITelegramBotClient botClient, long chatId)
    {
        await botClient.SendPhotoAsync(chatId, new InputMedia(_stream, "screenshot.png"));
    }
}