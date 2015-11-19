using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class TileFactory<TTile> : ITileFactory
        where TTile : ITile, new()
    {
        public ITile Create()
        {
            return new TTile();
        }
    }
}
