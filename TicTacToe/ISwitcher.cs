using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface ISwitcher
    {
        void Switch<T>(ref T item1, ref T item2);
    }
}
