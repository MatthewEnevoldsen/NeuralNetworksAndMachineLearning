using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TieDetector : ITieDetector
    {
        public bool IsGameTied(IGrid grid)
        {
            return grid.Count(t => t.Content == TileContent.Empty) == 0;
        }
    }
}
