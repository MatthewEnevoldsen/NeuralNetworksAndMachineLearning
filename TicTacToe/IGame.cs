using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IGame
    {
        IGameState State { get; }
        IFinishedGame PlayGame();
        void PerformMove();
        bool IsFinished { get; }
        IPlayer Winner { get; }
    }
}
