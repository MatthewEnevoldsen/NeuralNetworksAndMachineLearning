using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Engine.Network.Activation;

namespace EncogPlayground.TicTacToe
{
    [TestClass]
    public class TicTacToeGameTests
    {


        [TestMethod]
        public void InitialBoardIsEmpty()
        {
            var board = new TicTacToeBoard();
        }

        [TestMethod]
        public void FirstPieceIsX()
        {

        }

        [TestMethod]
        public void FirstTurnCanBeAnywhere()
        {

        }

        [TestMethod]
        public void CannotGoInSamePlaceTwice()
        {

        }

        [TestMethod]
        public void CannotPlace2XsAtOnce()
        {


        }

        [TestMethod]
        public void CannotPlace2OsAtOnce()
        {

        }
    }
}
