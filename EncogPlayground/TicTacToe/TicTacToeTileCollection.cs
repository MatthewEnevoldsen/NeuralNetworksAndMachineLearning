using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground.TicTacToe
{
    public class TicTacToeTileCollection : IEnumerable<TicTacToeTile>
    {
        public TicTacToeTile[,] _tiles;

        public TicTacToeTileCollection()
        {
            _tiles = new TicTacToeTile[3, 3];
            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                    _tiles[x, y] = new EmptyTicTacToeTile(x, y);
        }

        public TicTacToeTile this[Point p]
        {
            get
            {
                return this[p.X, p.Y];
            }
            set
            {
                this[p.X, p.Y] = value;
            }
        }

        public TicTacToeTile this[int x, int y]
        {
            get
            {
                return _tiles[x, y];
            }
            set
            {
                _tiles[x, y] = value;
            }
        }

        public IEnumerator<TicTacToeTile> GetEnumerator()
        {
            return _tiles.Cast<TicTacToeTile>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<TicTacToeTile>)this).GetEnumerator();
        }
    }
}
