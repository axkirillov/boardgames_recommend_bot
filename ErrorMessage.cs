using System.Threading;
using Telegram.Bot.Args;

namespace boardgame_bot
{
    internal static class ErrorMessage
    {
        internal static async void NotANumber(long id)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: id,
              text: "Please enter a number");
        }

        internal static async void Zero(string v, long id)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: id,
              text: $"{v} must be bigger than zero");
        }
    }
}