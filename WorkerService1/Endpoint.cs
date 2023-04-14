using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace WorkerService1;

public class Endpoint
{
    private readonly IEndpointAttribute _attribute;
    private readonly MethodInfo _method;

    public Endpoint(IEndpointAttribute attribute, MethodInfo method)
    {
        _attribute = attribute;
        _method = method;
    }

    public async Task Run(IServiceProvider serviceProvider, Update update, ITelegramBotClient bot)
    {
        var parameters = _method.GetParameters()
            .Select(info =>
            {
                if (info.CustomAttributes.Any(a => a.AttributeType == typeof(FromServicesAttribute)))
                {
                    return serviceProvider.GetService(info.ParameterType);
                }

                if (info.ParameterType == typeof(Update)) return update;
                return null;
            })
            .ToArray();

        var instance = serviceProvider.GetService(_method.DeclaringType);
        if (instance is not ViewController controller)
        {
            return;
        }

        controller.Update = update;
        var taskObj = _method.Invoke(instance, parameters);

        try
        {
            if (taskObj is Task<View> viewTask)
            {
                var view = await viewTask;
                var keyboard = controller.ToKeys();
                await bot.SendView(view, update.Message.Chat.Id, keyboard);
            }

            if (taskObj is Task<ImageContent> imageTask)
            {
                var view = await imageTask;
                await view.SendAsync(bot, update.Message.Chat.Id);
            }

            if (taskObj is Task task)
            {
                await task;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public bool IsSuitable(Update update) => _attribute.IsSuitable(update);
}