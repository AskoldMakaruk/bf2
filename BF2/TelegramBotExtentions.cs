using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace BF2;

public static class TelegramBotExtentions
{
    public static async Task SendView(this ITelegramBotClient bot, View view, long chatId, ReplyMarkupBase replyMarkupBase)
    {
        await bot.SendTextMessageAsync(chatId, view.ToView(), replyMarkup: replyMarkupBase);
    }
}