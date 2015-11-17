using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IGrid : IEnumerable<ITile>
    {
        ITile this[Point pos] { get; }
        ITile this[int x, int y] { get; }
        int Size { get; }
    }
}
