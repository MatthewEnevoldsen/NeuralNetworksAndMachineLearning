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
            for (int x = 0; x < grid.Size; x++)
                for (int y = 0; y < grid.Size; y++)
                    if (grid[x, y].Content != TileContent.Empty)
                        yield return new Point(x, y);
        }
    }
}
