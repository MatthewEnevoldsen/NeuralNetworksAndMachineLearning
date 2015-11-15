using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IPlayer
    {
        string Name { get; }
        Piece Piece { get; }
        void PerformMove();
    }
}
