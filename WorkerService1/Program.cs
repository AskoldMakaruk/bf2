using Autofac;
using Autofac.Extensions.DependencyInjection;
using BF2;
using HtmlAgilityPack;
using Telegram.Bot;

var client = new HttpClient();
var request = new HttpRequestMessage(HttpMethod.Get, "https://rezka.ag/search/?do=search&subaction=search&q=john-wick");
request.Headers.Add("UserAgent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.0.0 Safari/537.36");
// request.Headers.Add("Cookie", "PHPSESSID=i66bqhbr884sq14qifnr5gkn72; dle_user_token=ce9d7c0ec14fcba6aad37375967aef79");
var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();

var html = new HtmlDocument();
html.LoadHtml(await response.Content.ReadAsStringAsync());
var nodeWithSearchResult = html.DocumentNode.SelectSingleNode("//*[@id='main']/div[4]/div[2]/div/div[1]");
foreach (var filmNode in nodeWithSearchResult.ChildNodes)
{
    Console.WriteLine(filmNode.InnerHtml);
}

Console.WriteLine(await response.Content.ReadAsStringAsync());

var builder = Host.CreateDefaultBuilder(args);
builder.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(container => { })
    .ConfigureServices(services => { services.UseBF2(provider => provider.GetService<IConfiguration>()!["BotToken"]!); });

var app = builder.Build();
app.Run();

namespace WorkerService1
{
    // content + buttons = view

    public class StartController : BotController
    {
        [Command("/start")]
        public async Task<View> Start(TelegramBotClient client)
        {
            return new StartView(Update.Message.From.Username);
        }

        [Button("Help")]
        public async Task<View> Help()
        {
            return null;
        }
    }


    public class HelpView : View
    {
        public override string Format { get; }

        public string ToView()
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

        public override string Format { get; }

        public string ToView()
        {
            return $"Hi, {_userName}";
        }
    }
}