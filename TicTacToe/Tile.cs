using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Tile : ITile
    {
        public Tile()
        {
            Content = TileContent.Empty;
        }

        public TileContent Content
        {
            get; private set;
        }

        public void PlacePiece(Piece piece)
        {
            Content = piece;
        }
    }
}
