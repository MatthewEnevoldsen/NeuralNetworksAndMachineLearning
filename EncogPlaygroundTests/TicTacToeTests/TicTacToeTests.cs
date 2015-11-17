using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe;
using Moq;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace EncogPlaygroundTests.TicTacToeTests
{
    [TestClass]
    public class TicTacToeTests
    {
        [TestClass]
        public class TilesShould
        {
            private ITile GetTile()
            {
                return new Tile();
            }

            [TestMethod]
            public void DefaultToEmpty()
            {
                Assert.AreEqual(TileContent.Empty, GetTile().Content);
            }

            [TestMethod]
            public void TileContentChangeable()
            {
                var tile = GetTile();
                foreach (var piece in new[] { Piece.X, Piece.O })
                {
                    tile.PlacePiece(piece);
                    Assert.AreEqual(piece, tile.Content);
                }
            }
        }

        [TestClass]
        public class SwitcherShould
        {
            private ISwitcher GetSwitcher()
            {
                return new Switcher();
            }

            [TestMethod]
            public void SwitchRefTypes()
            {
                var item1 = "a";
                var item2 = "b";
                GetSwitcher().Switch(ref item1, ref item2);
                Assert.AreEqual("a", item2);
                Assert.AreEqual("b", item1);
            }

            [TestMethod]
            public void SwitchValTypes()
            {
                var item1 = 1;
                var item2 = 2;
                GetSwitcher().Switch(ref item1, ref item2);
                Assert.AreEqual(1, item2);
                Assert.AreEqual(2, item1);
            }
        }

        [TestClass]
        public class GridShould
        {
            private IGrid GetGrid()
            {
                var tf = new Mock<ITileFactory>();
                tf.Setup(t => t.Create()).Returns(new Mock<ITile>().Object);
                return new Grid(tf.Object);
            }

            private IEnumerable<Point> GetPoints()
            {
                return Enumerable.Range(0, Grid.SIZE).SelectMany(x => Enumerable.Range(0, Grid.SIZE).Select(y => new Point(x, y)));
            }

            [TestMethod]
            public void CreateTiles()
            {
                foreach (var t in GetGrid())
                    Assert.AreNotEqual(null, t);
            }

            [TestMethod]
            public void PointIndexerShouldHitEverything()
            {
                var grid = GetGrid();
                foreach (var p1 in GetPoints())
                    Assert.IsTrue(grid.Contains(grid[p1]));
            }

            [TestMethod]
            public void IntIntIndexerShouldHitEverything()
            {
                var grid = GetGrid();
                foreach (var p1 in GetPoints())
                    Assert.IsTrue(grid.Contains(grid[p1.X, p1.Y]));

            }

            [TestMethod]
            public void SizeShouldMatchGridSize()
            {
                var grid = GetGrid();
                Assert.AreEqual(grid.Size * grid.Size, grid.Count());
            }

            [TestMethod]
            public void ShouldPlacePiece()
            {
                var tileMock = new Mock<ITile>();
                var tf = new Mock<ITileFactory>();
                tf.Setup(t => t.Create()).Returns(tileMock.Object);
                var grid = new Grid(tf.Object);

                grid.PlacePiece(new Move(new Point(0, 0), Piece.X));   
                             
                tileMock.Verify(t => t.PlacePiece(Piece.X));
            }
        }
    }
}
