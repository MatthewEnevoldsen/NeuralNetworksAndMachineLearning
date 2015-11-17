using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Grid : IGrid
    {
        public const int SIZE = 3;

        private ITile[,] _tiles;

        public Grid(ITileFactory tileFactory)
        {
            _tiles = new ITile[SIZE, SIZE];
            for (int x = 0; x < SIZE; x++)
                for (int y = 0; y < SIZE; y++)
                    _tiles[x, y] = tileFactory.Create();
        }

        public ITile this[Point pos]
        {
            get
            {
                return this[pos.X, pos.Y];
            }
        }

        public ITile this[int x, int y]
        {
            get
            {
                return _tiles[x, y];
            }
        }

        public int Size
        {
            get
            {
                return SIZE;
            }
        }

        public IEnumerator<ITile> GetEnumerator()
        {
            return _tiles.Cast<ITile>().GetEnumerator();
        }

        public void PlacePiece(Move move)
        {
            this[move.Pos].PlacePiece(move.Piece);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ITile>)this).GetEnumerator();
        }
    }
}
