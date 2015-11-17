using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class NoWinnerAssumedTieDectector : ITieDetector
    {
        public bool IsGameTied(IGrid grid)
        {
            return grid.All(t => t.Content != TileContent.Empty);
        }
    }
}
