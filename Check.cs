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
                    if (game.MinPlayTime < 60)
                    {
                        // remain true
                    }
                    else
                    {
                        Result = false;
                    }
                    break;
                case "1-2":
                    if (game.MinPlayTime <= 120 && game.MaxPlayTime > 60)
                    {
                        // remain true
                    }
                    else
                    {
                        Result = false;
                    }
                    break;
                case ">2":
                    if (game.MaxPlayTime > 120)
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
        internal Check Age(){
            return this;
        }
    }
}