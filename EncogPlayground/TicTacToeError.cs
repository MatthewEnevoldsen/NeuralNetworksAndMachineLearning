using Encog.Neural.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encog.ML.Data;
using TicTacToe;

namespace EncogPlayground
{
    class TicTacToeError : IErrorFunction
    {

        IGrid _grid;
        IValidMoveGetter _validMoveGetter;
        bool _win;

        public void CalculateError(IMLData ideal, double[] actual, double[] error)
        {
            int moveIndex = Array.IndexOf(actual, actual.Max());
            if (_validMoveGetter.GetEmptySquares(_grid).Select(p => p.X * 3 + p.Y).Contains(moveIndex))
            {
                //need to know if we win
            }
            else
            {
                for (int i = 0; i < error.Length; i++)
                {
                    error[i] = double.MaxValue;//YOU FUCKED UP AI
                }
            }
        }
    }
}
