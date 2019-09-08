using System;
using Telegram.Bot.Args;

namespace boardgame_bot
{
    public class State
    {
        public long ChatId;
        public string Identifier;
        private int? numberOfPlayers;
        private string playTime;
        private int age;
        private int index = 0;
        private string[] States = {
             null,
             "number of players",
             "play time",
             "age",
             "result"
            };

        internal void Next()
        {
            index++;
            Identifier = States[index];
        }
        public string PlayTime { get => playTime; }
        public int? NumberOfPlayers { get => numberOfPlayers; }
        public int Age { get => age; }

        internal bool SetNumberOfPlayers(MessageEventArgs e, State state)
        {
            try
            {
                int result = Int32.Parse(e.Message.Text);
                if (result <= 0)
                {
                    ErrorMessage.Zero("Number of players", state.ChatId);
                    return false;
                }
                else
                {
                    numberOfPlayers = result;
                    return true;
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMessage.NotANumber(state.ChatId);
                return false;
            }
        }
        internal void SetPlayTime(string data)
        {
            playTime = data;
        }

        internal bool SetAge(MessageEventArgs e, State state)
        {
            try
            {
                int result = Int32.Parse(e.Message.Text);
                if (result <= 0)
                {
                    ErrorMessage.Zero("Age", state.ChatId);
                    return false;
                }
                else
                {
                    age = result;
                    return true;
                }
            }
            catch (FormatException ex)
            {
                Console.WriteLine(ex.Message);
                ErrorMessage.NotANumber(state.ChatId);
                return false;
            }
        }
    }
}