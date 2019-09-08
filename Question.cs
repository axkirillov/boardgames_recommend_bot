using System.Threading;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace boardgame_bot
{
    public class Question
    {
        public static void Initialize(MessageEventArgs e, State state)
        {
            if (e.Message.Text == "/start")
            {
                Message.Players(e);
                // state.Identifier = "HowManyPlayers";
                state.Next();
            }
            else
            {
                Message.Start(e);
            }
        }
        internal static async void PlayTime(State state)
        {
            InlineKeyboardButton[] row = {
                InlineKeyboardButton.WithCallbackData("< 1 hour", "<1"),
                InlineKeyboardButton.WithCallbackData("1-2 hours", "1-2"),
                InlineKeyboardButton.WithCallbackData("> 2 hours", ">2"),
            };
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: state.ChatId,
              text: "How long do you want your playing session to be?",
              replyMarkup: new InlineKeyboardMarkup(row)
            );
        }
        internal static async void Age(State state)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: state.ChatId,
              text: "How old is the youngest player?"
            );
        }
    }
}