using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground.TicTacToe
{
    public abstract class TicTacToeTile
    {
        public int XPos { get { return Pos.X; } }
        public int YPos { get { return Pos.Y; } }

        public Point Pos { get; private set; }

        public TicTacToeTile(int xPos, int yPos)
        {
            Pos = new Point(xPos, yPos);
        }

        public TicTacToeTile(Point pos)
        {
            Pos = pos;
        }
    }

    public class EmptyTicTacToeTile : TicTacToeTile
    {
        public EmptyTicTacToeTile(int xPos, int yPos)
            : base(xPos, yPos)
        { }

        public FilledTicTacToeTile PlacePiece(TicTacToePiece piece)
        {
            return new FilledTicTacToeTile(XPos, YPos, piece);
        }
    }

    public class FilledTicTacToeTile : TicTacToeTile
    {
        public TicTacToePiece Piece { get; private set; }

        public FilledTicTacToeTile(int xPos, int yPos, TicTacToePiece piece)
            : base(xPos, yPos)
        {
            Piece = piece;
        }
    }

    public enum TicTacToePiece
    {
        O, X
    }
}
