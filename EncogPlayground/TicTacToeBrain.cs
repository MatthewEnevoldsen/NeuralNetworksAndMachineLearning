using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe;
using Encog.Engine.Network.Activation;
using Encog.Neural.Networks.Training.Propagation.Back;
using Encog.ML.Data.Basic;
using Encog.ML.Data;
using System.Drawing;
using System.Threading;
using System.IO;

namespace EncogPlayground
{
    public class TicTacToeBrain : IPlayer
    {
        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Piece Piece
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Move GetMove(IGrid grid)
        {
            throw new NotImplementedException();
        }


        public void CanItLearnTheRules()
        {
            var p = Parallel.For(1, 15, i =>
            {
                var nn = new BasicNetwork();
                nn.AddLayer(new BasicLayer(new ActivationTANH(), false, 9));//input the grid contents
                for (int j = 0; j < i; j++)
                {
                    nn.AddLayer(new BasicLayer(new ActivationTANH(), true, i));
                }
                nn.AddLayer(new BasicLayer(new ActivationTANH(), false, 9));//next square to put a piece (x,y)
                nn.Structure.FinalizeStructure();
                nn.Reset();

                var p1 = new RecordingPlayer(new RandomPlayer("first", Piece.X, new ValidMoveGetter()));
                var p2 = new RecordingPlayer(new FirstPlayer("second", Piece.O, new ValidMoveGetter()));

                new GameRunner().RunGames(p1, p2, 1000);

                var trainingData = GetList(p2.AllMovesMade.Take(900));
                var verificationData = GetList(p2.AllMovesMade.Skip(900).Take(100));

                var dataSet = new BasicMLDataSet(trainingData);

                var train = new Backpropagation(nn, dataSet, 0.7, 0.3);
                train.BatchSize = 1;
                int epoch = 1;
                var sb = new StringBuilder();
                do
                {
                    train.Iteration();
                    epoch++;
                    if (epoch % 1000 == 0)
                        sb.AppendLine("Error: " + train.Error);
                } while (train.Error > 0.01 && epoch < 100000);

                foreach (var verf in verificationData)
                {
                    var output = nn.Compute(verf.Input);
                    int good = 0;
                    int bad = 0;
                    if (Math.Round(output[0]) == Math.Round(verf.Ideal[0]) && Math.Round(output[1]) == Math.Round(verf.Ideal[1]))
                    {
                        good++;
                    }
                    else
                    {
                        bad++;
                    }
                    sb.AppendLine(string.Format("Good {0} bad {1} ", good, bad));
                }
                var path = @"C:\Users\matte\Desktop\Results\" + i + ".txt";
                File.WriteAllText(path, sb.ToString());
            });

            while (!p.IsCompleted)
                Thread.Sleep(10000);
            Console.Read();

        }

        private int IndexOfMove(IMLData data)
        {
            double currentMax = double.MinValue;
            int currentMaxIndex = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i] > currentMax)
                {
                    currentMax = data[i];
                    currentMaxIndex = i;
                }
            }
            return currentMaxIndex;
        }

        private IList<IMLDataPair> GetList(IEnumerable<Tuple<int[,], Point>> input)
        {
            return input.Select(t => new BasicMLDataPair(
                new BasicMLData(t.Item1.Cast<int>().Select(i => (double)i).ToArray()),
                new BasicMLData(Enumerable.Range(0, 9).Select(i => i == t.Item2.X * 3 + t.Item2.Y ? 1.0 : 0.0).ToArray()))).Cast<IMLDataPair>().ToList();
        }

    }

    public class RecordingPlayer : IPlayer
    {
        private IPlayer _player;
        public List<Tuple<int[,], Point>> AllMovesMade = new List<Tuple<int[,], Point>>();

        public RecordingPlayer(IPlayer player)
        {
            _player = player;
        }

        public string Name
        {
            get
            {
                return _player.Name;
            }
        }

        public Piece Piece
        {
            get
            {
                return _player.Piece;
            }
        }

        public Move GetMove(IGrid grid)
        {
            var move = _player.GetMove(grid);
            int[,] gridState = new int[grid.Size, grid.Size];
            for (int y = 0; y < grid.Size; y++)
                for (int x = 0; x < grid.Size; x++)
                    gridState[x, y] = grid[x, y].Content.GetHashCode();
            AllMovesMade.Add(Tuple.Create(gridState, move.Pos));
            return move;
        }


    }
}
