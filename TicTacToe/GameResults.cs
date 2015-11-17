using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class GameResults
    {
        public bool IsFinished { get; set; }
        public IPlayer Winner { get; set; }
    }
}
