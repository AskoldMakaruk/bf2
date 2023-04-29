using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace BF2;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection UseBF2(this IServiceCollection services, Func<IServiceProvider, string> botTokenProvider)
    {
        var controllers = TypeHelper.GetControllerTypes();
        foreach (var controller in controllers)
        {
            services.AddScoped(typeof(BotController), controller);
            services.AddScoped(controller);
        }

        services.AddSingleton<ITelegramBotClient, TelegramBotClient>(provider => new(botTokenProvider(provider)));
        services.AddSingleton<EndpointCollection>(provider =>
        {
            var controllers = provider.GetServices<BotController>();
            var endpoints = controllers.SelectMany(controller => controller.GetEndpoints());
            return new EndpointCollection(endpoints);
        });

        services.AddHostedService<HostedBot>();

        return services;
    }
}