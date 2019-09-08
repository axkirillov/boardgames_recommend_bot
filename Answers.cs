using System;

namespace boardgame_bot
{
    public class Answers
    {
        public long ChatId;
        public string Identifier;
        public Nullable<int> NumberOfPlayers;
        private string playTime;
        private int index = 0;
        private string[] States = {
             null,
             "await number of players",
             "number of players is set",
             "Ask Play Time",
             "Give Result"
            };

        internal void Next()
        {
            index++;
            Identifier = States[index];
        }
        public string PlayTime { get => playTime; }

        internal void SetPlayTime(string data)
        {
            playTime = data;
        }
    }
}