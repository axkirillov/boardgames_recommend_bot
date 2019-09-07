using System;

namespace boardgame_bot
{
    internal class Check
    {
        private Game game;
        private Answers state;

        public bool Result;

        public Check(Game game, Answers state)
        {
            this.game = game;
            this.state = state;
        }

        internal Check Players()
        {
            if ((game.MinPlayers <= state.NumberOfPlayers) && (game.MaxPlayers >= state.NumberOfPlayers))
            {
                Result = true;
            }
            return this;
        }

        internal Check PlayTime()
        {
            Result = true;
            return this;
        }
    }
}