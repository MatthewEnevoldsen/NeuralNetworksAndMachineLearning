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

        private static Mock<IGrid> GetMockedGrid(IEnumerable<Point> xs, IEnumerable<Point> os)
        {
            var grid = new Mock<IGrid>();
            grid.SetupGet(g => g.Size).Returns(3);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var pos = new Point(x, y);
                    var tile = new Mock<ITile>();
                    tile.SetupGet(t => t.Content).Returns(xs.Contains(pos) ? Piece.X : os.Contains(pos) ? Piece.O : TileContent.Empty);
                    grid.SetupGet(g => g[x, y]).Returns(tile.Object);
                    grid.SetupGet(g => g[new Point(x, y)]).Returns(tile.Object);
                }
            }
            return grid;
        }

        [TestClass]
        public class WinDectectorShould
        {
            private IWinDetector GetDetector()
            {
                return new WinDectector();
            }

            private void EnsureResult(IEnumerable<Point> xs, IEnumerable<Point> os, Piece winner)
            {
                var grid = GetMockedGrid(xs, os);
                Assert.AreEqual(winner, GetDetector().GetWinner(grid.Object));
            }

            private void Ensure3PointsDetected(Point p1, Point p2, Point p3)
            {
                EnsureResult(new[] { p1, p2, p3 }, new Point[] { }, Piece.X);
                EnsureResult(new Point[] { }, new[] { p1, p2, p3 }, Piece.O);
            }

            [TestMethod]
            public void DetectTopLine()
            {
                Ensure3PointsDetected(new Point(0, 0), new Point(0, 1), new Point(0, 2));
            }
            [TestMethod]
            public void DetectHorMiddleLine()
            {
                Ensure3PointsDetected(new Point(1, 0), new Point(1, 1), new Point(1, 2));
            }
            [TestMethod]
            public void DetectDiag()
            {
                Ensure3PointsDetected(new Point(0, 0), new Point(1, 1), new Point(2, 2));
            }
            [TestMethod]
            public void DetectReversDiag()
            {
                Ensure3PointsDetected(new Point(2, 0), new Point(1, 1), new Point(0, 2));
            }
            [TestMethod]
            public void NotCountBlockedRunsAsWinner()
            {
                EnsureResult(new[] { new Point(0, 0), new Point(1, 0) }, new[] { new Point(2, 0) }, null);
            }
        }

        [TestClass]
        public class TieDetectorShould
        {
            private ITieDetector GetDetector()
            {
                return new TieDetector();
            }

            [TestMethod]
            public void AllTilesFilledIsTie()
            {
                var grid = new Mock<IGrid>();
                var tile = new Mock<ITile>();
                tile.Setup(t => t.Content).Returns(Piece.X);
                grid.Setup(g => g.GetEnumerator()).Returns(Enumerable.Range(0, 9).Select(i => tile.Object).GetEnumerator());
                Assert.IsTrue(GetDetector().IsGameTied(grid.Object));
            }

            [TestMethod]
            public void EightTilesIsNotTie()
            {
                var grid = new Mock<IGrid>();
                var tileX = new Mock<ITile>();
                tileX.Setup(t => t.Content).Returns(Piece.X);
                var emptyTile = new Mock<ITile>();
                emptyTile.Setup(t => t.Content).Returns(TileContent.Empty);
                grid.Setup(g => g.GetEnumerator()).Returns(Enumerable.Range(0, 8).Select(i => tileX.Object).Concat(new[] { emptyTile.Object }).GetEnumerator());
                Assert.IsFalse(GetDetector().IsGameTied(grid.Object));
            }
        }
        [TestClass]
        public class TileFactoryShould
        {
            private ITileFactory GetFactory()
            {
                return new TileFactory<Tile>();
            }

            [TestMethod]
            public void CreateTile()
            {
                Assert.AreNotEqual(null, GetFactory().Create());
            }

            [TestMethod]
            public void CreateDifferentInstances()
            {
                var fact = GetFactory();
                Assert.AreNotEqual(fact.Create(), fact.Create());
            }
        }
    }
}
