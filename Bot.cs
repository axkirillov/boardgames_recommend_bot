using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;


namespace boardgame_bot
{
    public class Bot
    {
        static ITelegramBotClient botClient;
        internal static void Run(string token)
        {
            botClient = new TelegramBotClient(token);
            var me = botClient.GetMeAsync().Result;
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                if (e.Message.Text == "/start")
                {
                    Thread.Sleep(1000);
                    await botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "How many people are going to play?");
                }
                else
                {
                    Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                    Console.WriteLine(e.Message);
                    Thread.Sleep(1000);
                    await botClient.SendTextMessageAsync(
                      chatId: e.Message.Chat,
                      text: "Hello, want me to recommend you some boardgames? Press /start");
                }
            }
        }
    }

}