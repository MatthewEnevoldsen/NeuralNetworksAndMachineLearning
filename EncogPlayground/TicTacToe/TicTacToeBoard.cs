using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground.TicTacToe
{
    public class TicTacToeBoard
    {
        private TicTacToeTileCollection _tiles;
        private TicTacToePiece _nextPiece;

        public TicTacToeBoard()
        {
            _tiles = new TicTacToeTileCollection();
            _nextPiece = TicTacToePiece.X;
        }

        public IEnumerable<TicTacToeMove> GetValidMoves()
        {
            return _tiles.Cast<EmptyTicTacToeTile>().Select(t=>new TicTacToeMove(t, _nextPiece));
        }

        public IEnumerable<TicTacToeTile> GetCurrnetState()
        {
            return _tiles;
        }
        
        public void PerformMove(TicTacToeMove move)
        {
            _tiles[move.Tile.Pos] = move.PerformMove();
        }
    }

    public class TicTacToeMove
    {
        public TicTacToePiece Piece { get; private set; }
        public EmptyTicTacToeTile Tile { get; private set; }

        public TicTacToeMove(EmptyTicTacToeTile tile, TicTacToePiece piece)
        {
            Tile = tile;
            Piece = piece;
        }

        public FilledTicTacToeTile PerformMove()
        {
            return Tile.PlacePiece(Piece);
        }
    }
}
