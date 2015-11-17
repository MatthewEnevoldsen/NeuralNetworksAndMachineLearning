using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class ValidMoveGetter : IValidMoveGetter
    {
        public IEnumerable<Point> GetEmptySquares(IGrid grid)
        {
            for (int x = 0; x < grid.Tiles.GetLength(0); x++)
                for (int y = 0; y < grid.Tiles.GetLength(1); y++)
                    if (grid.Tiles[x, y].Content != TileContent.Empty)
                        yield return new Point(x, y);
        }
    }
}
