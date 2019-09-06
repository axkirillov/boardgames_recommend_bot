using System;

namespace boardgame_bot
{
    public class Answers
    {
        public long ChatId;
        public string Identifier;
        public Nullable<int> NumberOfPlayers;
        private string playTime;

        public string PlayTime { get => playTime; }

        internal void SetPlayTime(string data)
        {
            playTime = data;
        }
    }
}