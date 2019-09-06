using System;

namespace boardgame_bot
{
    public class Answers
    {
        public string Identifier;
        public Nullable<int> NumberOfPlayers;
        private Nullable<int> playTime;

        public int? PlayTime { get => playTime; }

        internal void SetPlayTime(string data)
        {
            playTime = int.Parse(data);
        }
    }
}