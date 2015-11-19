using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class RandomPlayer : IPlayer
    {
        private IValidMoveGetter _moveGetter;
        private static Random rand = new Random(100);

        public RandomPlayer(string name, Piece piece, IValidMoveGetter moveGetter)
        {
            Name = name;
            Piece = piece;
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

        public Move GetMove(IGrid grid)
        {
            var moves = _moveGetter.GetEmptySquares(grid).ToArray();
            var skip = rand.Next(0, moves.Count());
            return new Move(moves.Skip(skip).First(), Piece);
        }
    }

    public class FirstPlayer : IPlayer
    {
        private IValidMoveGetter _moveGetter;
        private static Random rand = new Random(100);

        public FirstPlayer(string name, Piece piece, IValidMoveGetter moveGetter)
        {
            Name = name;
            Piece = piece;
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

        public Move GetMove(IGrid grid)
        {
            var moves = _moveGetter.GetEmptySquares(grid).ToArray();
            return new Move(moves.First(), Piece);
        }
    }
}
