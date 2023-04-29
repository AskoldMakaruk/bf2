using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BF2;

public class HostedBot : IHostedService
{
    private readonly TelegramBotClient _botClient;
    private readonly EndpointCollection _endpointCollection;
    private readonly IServiceProvider _serviceProvider;

    public HostedBot(TelegramBotClient botClient, EndpointCollection endpointCollection, IServiceProvider serviceProvider)
    {
        _botClient = botClient;
        _endpointCollection = endpointCollection;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _botClient.StartReceiving(UpdateHandler, PollingErrorHandler, cancellationToken: cancellationToken);
    }


    private async Task UpdateHandler(ITelegramBotClient arg1, Update update, CancellationToken arg3)
    {
        var endpoint = _endpointCollection.Suitable(update);
        if (endpoint is null)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        await endpoint.Run(scope.ServiceProvider, update, arg1);
    }

    private async Task PollingErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        Console.WriteLine(arg2);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}