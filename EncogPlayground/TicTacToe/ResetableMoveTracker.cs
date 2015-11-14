using SolidTacToe.Definitions;
using SolidTacToe.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground.TicTacToe
{
    class ResetableMoveTracker : IMoveTracker
    {
        public MoveTracker MoveTracker { get; set; }


        public IPlayer GetCurrentPlayer()
        {
            return MoveTracker.GetCurrentPlayer();
        }

        public IPlayer Next()
        {
            return MoveTracker.Next();
        }

        public void Reset()
        {
            MoveTracker = new MoveTracker();
        }
    }
}
