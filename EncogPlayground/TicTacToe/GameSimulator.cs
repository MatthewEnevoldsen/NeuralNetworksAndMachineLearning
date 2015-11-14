using SolidTacToe.Definitions;
using SolidTacToe.Exe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground.TicTacToe
{
    public class GameSimulator
    {
        public void SimulateGame(Action<IGameStatusCondition> winAction)
        {

            IGameStatusCondition condition = null;

            while (true)
            {
                if (condition != null) { break; }
                condition = TicTacToeBindings.Get<IGameRunner>().ExecuteTurn();
            }
            winAction(condition);
        }
    }
}
