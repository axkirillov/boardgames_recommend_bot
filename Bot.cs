using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace boardgame_bot
{
    public class Bot
    {
        public static ITelegramBotClient botClient;
        static StateStore stateStore;
        internal static void Run(string token)
        {
            stateStore = new StateStore();
            botClient = new TelegramBotClient(token);
            var me = botClient.GetMeAsync().Result;
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
        static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var state = GetState(e);
            AskQuestion(state, e);
            state = UpdateState(state, e);
            GiveResult(state, e);
        }

        private static Answers UpdateState(Answers state, MessageEventArgs e)
        {
            if (state.NumberOfPlayers == null && !state.WaitingForNumberOfPlayers)
            {
                state.WaitingForNumberOfPlayers = true;
            }
            else if (state.WaitingForNumberOfPlayers == true)
            {
                setNumberOfPlayers(ref state, e);
            }
        }

        private static async void GiveResult(Answers state, MessageEventArgs e)
        {
            if (state.NumberOfPlayers == null)
            {

            }
            else
            {
                Thread.Sleep(1000);
                await botClient.SendTextMessageAsync(
                  chatId: e.Message.Chat,
                  text: "You should play Monopoly!");
                EraseState(e);
            }
        }

        private static void EraseState(MessageEventArgs e)
        {
            stateStore.Remove(e.Message.Chat.Id);
        }

        private static void AskQuestion(Answers state, MessageEventArgs e)
        {
            if (state.NumberOfPlayers == null && !state.WaitingForNumberOfPlayers)
            {
                AskHowManyPlayers(e);
            }
        }

        private static void setNumberOfPlayers(ref Answers state, MessageEventArgs e)
        {
            try
            {
                int result = Int32.Parse(e.Message.Text);
                Console.WriteLine(result);
                if (result == 0)
                {
                    ErrorMessages.Zero("Number of players", e);
                }
                else
                {
                    state.NumberOfPlayers = result;
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMessages.NotANumber(e);
            }
        }

        private static async void AskHowManyPlayers(MessageEventArgs e)
        {
            Thread.Sleep(1000);
            await botClient.SendTextMessageAsync(
              chatId: e.Message.Chat,
              text: "How many people are going to play?");
        }

        private static Answers GetState(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            if (stateStore.ContainsKey(id))
            {
                return stateStore[id];
            }
            else
            {
                var state = new Answers();
                stateStore.Add(id, state);
                return state;
            }
        }
    }
}