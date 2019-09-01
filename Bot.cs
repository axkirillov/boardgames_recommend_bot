using System;
using System.Threading;
using Google.Cloud.Firestore;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace boardgame_bot
{
    public class Bot
    {
        public static ITelegramBotClient botClient;
        static StateStore stateStore;
        static CollectionReference games;
        internal static void Run(string token)
        {
            games = Firestore.Connect();
            stateStore = new StateStore();
            botClient = new TelegramBotClient(token);
            var me = botClient.GetMeAsync().Result;
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var state = GetState(e);
            switch (state.Identifier)
            {
                case null:
                    InitializeQuestions(e, state);
                    break;
                case "HowManyPlayers":
                    setNumberOfPlayers(e, state);
                    if (state.Identifier == "NumberOfPlayersSet")
                    {
                        var players = state.NumberOfPlayers;
                        Query checkForMax = games.WhereGreaterThanOrEqualTo("MaxPlayers", players).Limit(1);
                        QuerySnapshot checkForMaxSnap = await checkForMax.GetSnapshotAsync();
                        if (checkForMaxSnap.Documents.Count == 0)
                        {
                            Message.TooManyPlayers(e);
                        }
                        else
                        {
                            var i = 1;
                            NextResult(i, state, e);
                        }
                        EraseState(e);
                    }
                    break;
            }
        }

        private static async void NextResult(int i, Answers state, MessageEventArgs e)
        {
            var players = state.NumberOfPlayers;
            Query query = games
                        .OrderByDescending("Rating")
                        .Limit(i);
            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            if (snapshot.Documents.Count != 0)
            {
                Game game = snapshot[i - 1].ConvertTo<Game>();
                if ((game.MinPlayers <= players) && (game.MaxPlayers >= players))
                {
                    Console.WriteLine(game.Id);
                    Message.Recommend(e, game);
                }
                else
                {
                    i++;
                    NextResult(i, state, e);
                }

            }
        }

        private static void InitializeQuestions(MessageEventArgs e, Answers state)
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

        private static void EraseState(MessageEventArgs e)
        {
            stateStore.Remove(e.Message.Chat.Id);
        }

        private static void setNumberOfPlayers(MessageEventArgs e, Answers state)
        {
            try
            {
                int result = Int32.Parse(e.Message.Text);
                if (result <= 0)
                {
                    ErrorMessage.Zero("Number of players", e);
                }
                else
                {
                    state.NumberOfPlayers = result;
                    state.Identifier = "NumberOfPlayersSet";
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMessage.NotANumber(e);
            }
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