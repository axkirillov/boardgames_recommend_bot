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
                    Question.Initialize(e, state);
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
                            state.Identifier = "Ask Play Time";
                            Question.PlayTime(e, state);
                        }
                    }
                    break;
                case "Ask Play Time":
                    state.Identifier = "Give Result";
                    break;
            }
            if (state.Identifier == "Give Result")
            {
                DocumentSnapshot lastDocSnap = null;
                NextResult(lastDocSnap, state, e);
                EraseState(e);
            }
        }
        private static async void NextResult(DocumentSnapshot lastDocSnap, Answers state, MessageEventArgs e)
        {
            var players = state.NumberOfPlayers;
            Query query = null;
            if (lastDocSnap == null)
            {
                query = games
               .OrderByDescending("Rating")
               .Limit(100);
            }
            else
            {
                query = games
               .OrderByDescending("Rating").StartAfter(lastDocSnap)
               .Limit(100);
            }

            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            if (snapshot.Documents.Count != 0)
            {
                var found = false;
                foreach (DocumentSnapshot document in snapshot)
                {
                    Game game = document.ConvertTo<Game>();
                    if ((game.MinPlayers <= players) && (game.MaxPlayers >= players))
                    {
                        Console.WriteLine(game.Id);
                        Message.Recommend(e, game);
                        found = true;
                        break;
                    }
                }
                if (found == false)
                {
                    lastDocSnap = snapshot[99];
                    NextResult(lastDocSnap, state, e);
                }

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