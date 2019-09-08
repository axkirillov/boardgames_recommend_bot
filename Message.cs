using System;
using System.Threading;
using Telegram.Bot.Args;

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
            Thread.Sleep(1000);
            await Bot.botClient.SendPhotoAsync(
              chatId: id,
              photo: game.Image,
              caption: $"You should play {game.Name}!");
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