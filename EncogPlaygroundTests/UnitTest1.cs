using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EncogPlayground.TicTacToe;

namespace EncogPlaygroundTests
{
    [TestClass]
    public class TicTacToeTests
    {
        [TestMethod]
        public void SimulateSingleGame()
        {
            var sim = new GameSimulator();
            sim.SimulateGame((t) => { });
        }
        [TestMethod]
        public void SimulateSingleGame2()
        {
            var sim = new GameSimulator();
            sim.SimulateGame((t) => { });

        }
    }
}
