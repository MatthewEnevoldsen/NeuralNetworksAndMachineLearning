using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class GameRunner
    {
        public void RunGames(IPlayer playerOne, IPlayer playerTwo, int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                var game = new Game(new Switcher(),
                            new WinDectector(),
                            new TieDetector(),
                            new Grid(new TileFactory<Tile>()),
                            playerOne,
                            playerTwo);
                GameResults status;

                for (;;)
                {
                    status = game.PerformMove();
                    if (status.IsFinished)
                        break;
                }
            }

        }

    }
}
