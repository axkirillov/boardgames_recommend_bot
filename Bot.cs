using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;


namespace HelloWorld
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
                Console.WriteLine($"Received a text message in chat {e.Message.Chat.Id}.");
                Thread.Sleep(1000);
                await botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "Пашел нахуй");


            }
        }
    }

}