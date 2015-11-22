using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Piece : TileContent
    {
        public static readonly Piece X = new Piece(2);
        public static readonly Piece O = new Piece(3);

        protected Piece(int internalValue) : base(internalValue)
        {
        }
    }

    public class TileContent
    {
        public static readonly TileContent Empty = new TileContent(1);

        public int InternalValue { get; protected set; }

        protected TileContent(int internalValue)
        {
            this.InternalValue = internalValue;
        }

        public override bool Equals(object obj)
        {
            var asTc = obj as TileContent;
            return asTc != null && InternalValue == asTc.InternalValue;
        }

        public override int GetHashCode()
        {
            return InternalValue.GetHashCode();
        }

        public override string ToString()
        {
            return this == Piece.X ? "X" : this == Piece.O ? "O" : "_";
        }
    }
}

