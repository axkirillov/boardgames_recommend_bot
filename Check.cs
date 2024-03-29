using System;

namespace boardgame_bot
{
    internal class Check
    {
        private Game game;
        private State state;

        public bool Result;

        public Check(Game game, State state)
        {
            this.game = game;
            this.state = state;
            Result = true;
        }

        internal Check Players()
        {
            if ((game.MinPlayers <= state.NumberOfPlayers) && (game.MaxPlayers >= state.NumberOfPlayers))
            {
                // remain true
            }
            else
            {
                Result = false;
            }
            return this;
        }

        internal Check PlayTime()
        {
            switch (state.PlayTime)
            {
                case "<1":
                    if (game.MaxPlayTime <= 60)
                    {
                        // remain true
                    }
                    else
                    {
                        Result = false;
                    }
                    break;
                case "1-2":
                    if (game.MaxPlayTime <= 120 && game.MaxPlayTime > 60)
                    {
                        // remain true
                    }
                    else
                    {
                        Result = false;
                    }
                    break;
                case "2-5":
                    if (game.MaxPlayTime <= 300 && game.MaxPlayTime > 120)
                    {
                        // remain true
                    }
                    else
                    {
                        Result = false;
                    }
                    break;
                case ">5":
                    if (game.MaxPlayTime > 300)
                    {
                        // remain true
                    }
                    else
                    {
                        Result = false;
                    }
                    break;
            }
            return this;
        }
        internal Check Age()
        {
            if (game.MinAge <= state.Age)
            {
                // remain true
            }
            else
            {
                Result = false;
            }
            return this;
        }
    }
}