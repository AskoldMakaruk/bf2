using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using WorkerService1;

namespace BF2;

public abstract class BotController
{
    public Update Update;
    public TelegramBotClient Bot;

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