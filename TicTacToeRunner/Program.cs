using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TicTacToe;

namespace TicTacToeRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            int delay = 400;
            int xWins = 0;
            int oWins = 0;
            int ties = 0;

            var sw = Stopwatch.StartNew();
            var loop = Parallel.For(0, 1000 * 100, i =>
            {
                var game = new Game(new Switcher(),
                            new WinDectector(),
                            new TieDetector(),
                            new Grid(new TileFactory<Tile>()),
                            new FirstPlayer("1", Piece.X, new ValidMoveGetter()),
                            new RandomPlayer("2", Piece.O, new ValidMoveGetter()));
                GameResults status;

                for (;;)
                {
                    //Thread.Sleep(delay);
                    status = game.PerformMove();
                    //DrawGrid(game.Grid);
                    if (status.IsFinished)
                        break;
                }
                if (status.Winner == null)
                    Interlocked.Increment(ref ties);
                else if (status.Winner.Piece == Piece.X)
                    Interlocked.Increment(ref xWins);
                else
                    Interlocked.Increment(ref oWins);
                //Thread.Sleep(delay);
                //Console.WriteLine("winner: " + (status.Winner == null ? "Tie" : status.Winner.Name));
                //Thread.Sleep(delay);
            });
            while (!loop.IsCompleted)
                Thread.Sleep(1000);
            Console.WriteLine("Time: " + sw.ElapsedMilliseconds);
            Console.WriteLine("Ties: " + ties);
            Console.WriteLine("X: " + xWins);
            Console.WriteLine("O: " + oWins);
            Console.Read();
        }

        private static void DrawGrid(IGrid grid)
        {
            Console.Clear();
            for (int x = 0; x < grid.Size; x++)
            {
                for (int y = 0; y < grid.Size; y++)
                {
                    var value = grid[x, y].Content.InternalValue;
                    Console.Write(value == 1 ? "_" : value == 2 ? "X" : "O");
                }
                Console.WriteLine(Environment.NewLine);
            }
        }
    }
}