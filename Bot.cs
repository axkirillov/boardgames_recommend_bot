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
                case "play time":
                    state.SetPlayTime(e.CallbackQuery.Data);
                    Question.Age(state);
                    state.Next();
                    break;
            }
            if (state.Identifier == "result")
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
                case "number of players":
                    var setNumber = state.SetNumberOfPlayers(e, state);
                    if (setNumber)
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
                            state.Next();
                            Question.PlayTime(state);
                        }
                    }
                    break;
                    case "age":
                    state.Next();
                    break;
            }
            if (state.Identifier == "result")
            {
                DocumentSnapshot lastDocSnap = null;
                NextResult(lastDocSnap, state);
                EraseState(state.ChatId);
            }
        }
        private static async void NextResult(DocumentSnapshot lastDocSnap, State state)
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
                        .PlayTime().Age();
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

            } else {
                Message.NotFound(state.ChatId);
            }
        }

        private static void EraseState(long id)
        {
            stateStore.Remove(id);
        }

        private static State GetState(long id)
        {
            if (stateStore.ContainsKey(id))
            {
                return stateStore[id];
            }
            else
            {
                var state = new State();
                stateStore.Add(id, state);
                state.ChatId = id;
                return state;
            }
        }
    }
}