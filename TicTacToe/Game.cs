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

        public Game(IGameState state, IPlayer playerOne, IPlayer playerTwo, IWinDetector winDetector, ITieDetector tieDetector)
        {
            State = state;
            Winner = null;
            IsFinished = false;
            _winDetector = winDetector;
            _tieDetector = tieDetector;
        }

        public bool IsFinished
        {
            get; private set;
        }

        public IPlayer Winner
        {
            get; private set;

        }

        public IGameState State
        {
            get; private set;
        }       

        public void PerformMove()
        {
            var move = State.PlayerTracker.CurrentPlayer.GetMove(State);
            State.Grid.Tiles[move.Pos.X, move.Pos.Y].PlacePiece(move.Piece);
            State.PlayerTracker.SwitchPlayers();
            if (_tieDetector.IsGameTied(State))
                IsFinished = true;
            Winner = _winDetector.GetWinner(State);
            if (Winner != null)
                IsFinished = true;
        }


        public IPlayer PlayGame()
        {
            while (!IsFinished)
                PerformMove();
            return Winner;
        }
    }
}
