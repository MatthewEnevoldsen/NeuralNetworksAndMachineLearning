using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IGame
    {
        IGrid Grid { get; }
        PlayerTracker PlayerTracker { get; }
        GameResults PerformMove();
    }
}
