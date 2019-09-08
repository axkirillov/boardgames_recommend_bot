using System;
using System.Threading;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace boardgame_bot
{
    public class Message
    {
        internal static async void Players(MessageEventArgs e)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: "How many people are going to play?");
        }
        internal static async void Start(MessageEventArgs e)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: "Press /start to begin");
        }

        internal static async void Recommend(long id, Game game)
        {
            InlineKeyboardButton[] row1 = {
                InlineKeyboardButton.WithCallbackData("Give me another one", "another"),
            };
            InlineKeyboardButton[] row2 = {
                InlineKeyboardButton.WithCallbackData("Start over", "startover"),
            };
            InlineKeyboardButton[][] rows = { row1, row2 };
            Thread.Sleep(1000);
            Console.WriteLine(String.Join(Environment.NewLine,
            game.Name,
            game.MinPlayers + "-" + game.MaxPlayers + " players",
            game.MinPlayTime + "-" + game.MaxPlayTime + " min",
            "from " + game.MinAge + " y.o."
            ));
            await Bot.botClient.SendPhotoAsync(
              chatId: id,
              photo: game.Image,
              caption: $"You should play {game.Name}!",
              replyMarkup: new InlineKeyboardMarkup(rows)
            );
        }
        internal static async void TooManyPlayers(MessageEventArgs e)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: $"Sorry, we don't have a game for so many players");
        }
        internal static async void NotFound(long id)
        {
            Thread.Sleep(1000);
            await Bot.botClient.SendTextMessageAsync(
              chatId: id,
              text: $"Sorry, we could not a game that suits you");
        }
    }
}