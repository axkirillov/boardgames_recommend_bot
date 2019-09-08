using System;

namespace boardgame_bot
{
    public class State
    {
        public long ChatId;
        public string Identifier;
        public Nullable<int> NumberOfPlayers;
        private string playTime;
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

        internal void SetPlayTime(string data)
        {
            playTime = data;
        }
    }
}