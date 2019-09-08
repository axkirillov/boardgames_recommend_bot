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
                Question.Players(e.Message.Chat.Id);
                state.Next();
            }
            else
            {
                Message.Start(e);
            }
        }
        internal static async void Players(long id)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: id,
              text: "How many people are going to play?");
        }
        internal static async void PlayTime(State state)
        {
            InlineKeyboardButton[] row1 = {
                InlineKeyboardButton.WithCallbackData("< 1 hour", "<1"),
                InlineKeyboardButton.WithCallbackData("1-2 hours", "1-2"),
            };
            InlineKeyboardButton[] row2 = {
                InlineKeyboardButton.WithCallbackData("2-5 hours", "2-5"),
                InlineKeyboardButton.WithCallbackData("> 5 hours", ">5"),
            };
            InlineKeyboardButton[][] rows = { row1, row2 };
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: state.ChatId,
              text: "How long do you want your playing session to be?",
              replyMarkup: new InlineKeyboardMarkup(rows)
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