using System.Threading;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace boardgame_bot
{
    public class Question
    {
        public static void Initialize(MessageEventArgs e, Answers state)
        {
            if (e.Message.Text == "/start")
            {
                Message.Players(e);
                state.Identifier = "HowManyPlayers";
            }
            else
            {
                Message.Start(e);
            }
        }
        internal static async void PlayTime(MessageEventArgs e, Answers state)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: "How long do you want your playing session to be?",
              replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData(
                "60 min",
                "60"
              ))
            );
        }
    }
}