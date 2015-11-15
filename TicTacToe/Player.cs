using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Player : IPlayer
    {
        private IGameState _state;
        private IValidMoveGetter _moveGetter;

        public Player(string name, Piece piece, IGameState state, IValidMoveGetter moveGetter)
        {
            Name = name;
            Piece = piece;
            _state = state;
            _moveGetter = moveGetter;
        }

        public string Name
        {
            get; private set;
        }

        public Piece Piece
        {
            get; private set;
        }

        public void PerformMove()
        {
            var move = _moveGetter.GetMoves().First();
            _state.Grid.Tiles[move.Pos.X, move.Pos.Y].PlacePiece(move.Piece);
        }
    }
}
