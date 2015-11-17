using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class WinDectector : IWinDetector
    {
        public Piece GetWinner(IGrid grid)
        {
            foreach (var p in new[] { Piece.X, Piece.O })
                if (GetWinnableLines(grid.Size).Any(line => line.All(pos => p == grid[pos].Content)))
                    return p;
            return null;
        }

        private List<Point[]> GetWinnableLines(int gridSize)
        {
            var ztgs = ZeroToGridSize(gridSize).ToArray();
            var results = new List<Point[]>();
            results.AddRange(ztgs.Select(x => ztgs.Select(y => new Point(x, y)).ToArray()));//vert
            results.AddRange(ztgs.Select(y => ztgs.Select(x => new Point(x, y)).ToArray()));//hor
            results.Add(ztgs.Select(diag => new Point(diag, diag)).ToArray());
            results.Add(ztgs.Select(diag => new Point(diag, gridSize - 1 - diag)).ToArray());
            return results;
        }

        private IEnumerable<int> ZeroToGridSize(int gridSize)
        {
            return Enumerable.Range(0, gridSize);
        }
    }
}
