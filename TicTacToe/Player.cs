using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Player : IPlayer
    {
        private IValidMoveGetter _moveGetter;
        private IMoveFactory _moveFact;

        public Player(string name, Piece piece, IValidMoveGetter moveGetter, IMoveFactory moveFactory)
        {
            Name = name;
            Piece = piece;
            _moveGetter = moveGetter;
            _moveFact = moveFactory;
        }

        public string Name
        {
            get; private set;
        }

        public Piece Piece
        {
            get; private set;
        }

        public Move GetMove(IGrid grid)
        {
            return _moveFact.CreateMove(_moveGetter.GetEmptySquares(grid).First(), Piece);
        }
    }
}
