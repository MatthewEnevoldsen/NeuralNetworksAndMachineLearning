using EncogPlayground.TicTacToe;
using SolidTacToe.Definitions;
using SolidTacToe.Exe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground
{
    public class Program
    {
        static void Main()
        {
            int maxGames = 2;
            int ties = 0;
            int xWins = 0;
            int oWins = 0;

            var sw = Stopwatch.StartNew();

            for (int i = 0; i < maxGames; i++)
            {

                new GameSimulator().SimulateGame(condition =>
                {

                    var gameWon = condition as IGameWonCondition;
                    if (gameWon != null)
                    {
                        if (gameWon.Winner == SolidTacToe.Token.O)
                            oWins++;
                        else
                            xWins++;
                    }
                    else
                        ties++;
                });
            }

            Console.WriteLine("X won {0} times, O won {1} times, and there were {2} ties. Took {3} ms", xWins, oWins, ties, sw.ElapsedMilliseconds);
            Console.Read();
        }

    }
}
