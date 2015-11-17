using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Game : IGame
    {
        private IWinDetector _winDetector;
        private ITieDetector _tieDetector;
        private ISwitcher _switcher;

        public Game(ISwitcher switcher, IWinDetector winDetector, ITieDetector tieDetector, IGrid grid, IPlayer playerOne, IPlayer playerTwo)
        {
            _winDetector = winDetector;
            _tieDetector = tieDetector;
            _switcher = switcher;
            PlayerTracker = new PlayerTracker() { CurrentPlayer = playerOne, NextPlayer = playerTwo };
            Grid = grid;
        }

        public IGrid Grid
        {
            get; private set;
        }

        public PlayerTracker PlayerTracker
        {
            get; private set;
        }

        public GameResults PerformMove()
        {
            var move = PlayerTracker.CurrentPlayer.GetMove(Grid);
            Grid.PlacePiece(move);
            if (_winDetector.GetWinner(Grid) != null)
                return new GameResults() { IsFinished = true, Winner = PlayerTracker.CurrentPlayer };
            if (_tieDetector.IsGameTied(Grid))
                return new GameResults() { IsFinished = true, Winner = null };
            _switcher.Switch(ref PlayerTracker.CurrentPlayer, ref PlayerTracker.NextPlayer);
            return new GameResults() { IsFinished = false, Winner = null };
        }
    }
}
