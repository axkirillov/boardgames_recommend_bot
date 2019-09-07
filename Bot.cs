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
            botClient.OnCallbackQuery += Bot_OnCallbackQuery;
            botClient.StartReceiving();
            Thread.Sleep(int.MaxValue);
        }

        private static void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            long id = e.CallbackQuery.Message.Chat.Id;
            var state = GetState(id);
            switch (state.Identifier)
            {
                case "Ask Play Time":
                    state.SetPlayTime(e.CallbackQuery.Data);
                    Question.Age(state);
                    break;
            }
            if (state.Identifier == "Give Result")
            {
                DocumentSnapshot lastDocSnap = null;
                NextResult(lastDocSnap, state);
                EraseState(state.ChatId);
            }
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            long id = e.Message.Chat.Id;
            var state = GetState(id);
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
                            Question.PlayTime(state);
                        }
                    }
                    break;
            }
            if (state.Identifier == "Give Result")
            {
                DocumentSnapshot lastDocSnap = null;
                NextResult(lastDocSnap, state);
                EraseState(state.ChatId);
            }
        }
        private static async void NextResult(DocumentSnapshot lastDocSnap, Answers state)
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
                    var check = new Check(game,state);
                    check.Players()
                        .PlayTime();
                    if (check.Result == true)
                    {
                        found = true;
                        Message.Recommend(state.ChatId, game);
                        Console.WriteLine(game.Id);
                        break;
                    }
                }
                if (found == false)
                {
                    lastDocSnap = snapshot[99];
                    NextResult(lastDocSnap, state);
                }

            }
        }

        private static void EraseState(long id)
        {
            stateStore.Remove(id);
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

        private static Answers GetState(long id)
        {
            if (stateStore.ContainsKey(id))
            {
                return stateStore[id];
            }
            else
            {
                var state = new Answers();
                stateStore.Add(id, state);
                state.ChatId = id;
                return state;
            }
        }
    }
}