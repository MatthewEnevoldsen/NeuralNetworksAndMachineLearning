using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Move
    {
        public Move(Point pos, Piece piece)
        {
            Piece = piece;
            Pos = pos;
        }

        public Piece Piece { get; private set; }
        public Point Pos { get; private set; }
    }
}
