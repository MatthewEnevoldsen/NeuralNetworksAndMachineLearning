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
using System.Diagnostics;

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

            var p1 = new RecordingPlayer1801s(new RandomPlayer("first", Piece.X, new ValidMoveGetter()));
            var p2 = new RecordingPlayer1801s(new FirstPlayer("second", Piece.O, new ValidMoveGetter()));

            new GameRunner().RunGames(p1, p2, 10000);

            var inputData = GetList(p2.AllMovesMade).Take(9000).ToList();
            var verfData = GetList(p2.AllMovesMade).Skip(9000).Take(1000).ToList();


            var hiddenLayerCounts = new[] { 1, 2, 3, 5, 8, 13, 21, 34 };
            var neuronCounts = new[] { 2, 3, 5, 8, 13, 21, 34 };
            var actFuncs = new IActivationFunction[] { new ActivationTANH(), new ActivationSIN(), new ActivationSigmoid(), new ActivationLinear() };
            var learnRates = new[] { 0.1, 0.3, 0.7 };
            var momentums = new[] { 0.1, 0.3, 0.5 };
            var batchSizes = new[] { 1 };

            var par1 = Parallel.ForEach(hiddenLayerCounts, hlc =>
             {
                 var par2 = Parallel.ForEach(neuronCounts, nc =>
                 {
                     var p3 = Parallel.ForEach(actFuncs, af =>
                     {
                         var p4 = Parallel.ForEach(learnRates, lr =>
                         {
                             var p5 = Parallel.ForEach(momentums, mom =>
                             {
                                 var p6 = Parallel.ForEach(batchSizes, bat =>
                                 {
                                     CanItLearnRulesWith(inputData, verfData, hlc, nc, af, lr, mom, bat);

                                 });
                                 while (!p6.IsCompleted)
                                     Thread.Sleep(10000);
                             });
                             while (!p5.IsCompleted)
                                 Thread.Sleep(10000);
                         });
                         while (!p4.IsCompleted)
                             Thread.Sleep(10000);
                     });
                     while (!p3.IsCompleted)
                         Thread.Sleep(10000);
                 });
                 while (!par2.IsCompleted)
                     Thread.Sleep(10000);
             });
            while (!par1.IsCompleted)
                Thread.Sleep(10000);

            Console.Read();

        }

        private void CanItLearnRulesWith(IList<IMLDataPair> inputData, IList<IMLDataPair> verfData, int hiddenLayerCount, int neuronCount, IActivationFunction actFunc, double learnRate, double momentum, int batchSize)
        {
            var nn = new BasicNetwork();
            nn.AddLayer(new BasicLayer(new ActivationTANH(), false, inputData.First().Input.Count));//input the grid contents
            for (int j = 0; j < hiddenLayerCount; j++)
                nn.AddLayer(new BasicLayer(actFunc, true, neuronCount));
            nn.AddLayer(new BasicLayer(new ActivationTANH(), false, 9));//next square to put a piece 
            nn.Structure.FinalizeStructure();
            nn.Reset();

            var dataSet = new BasicMLDataSet(inputData);

            var train = new Backpropagation(nn, dataSet, learnRate, momentum);
            train.BatchSize = batchSize;
            int epoch = 1;
            var sb = new StringBuilder();
            var fileName = " hlc " + hiddenLayerCount + " nc " + neuronCount + " af " + actFunc.GetType().ToString() + " lr " + learnRate + " mom " + momentum + "batch " + batchSize;
            sb.AppendLine("hlc: " + hiddenLayerCount + ",");
            sb.AppendLine("nc: " + neuronCount + ",");
            sb.AppendLine("af: " + actFunc.GetType().ToString() + ",");
            sb.AppendLine("lr: " + learnRate + ",");
            sb.AppendLine("mom: " + momentum + ",");
            sb.AppendLine("batch: " + batchSize + ",");
            do
            {
                train.Iteration();
                epoch++;
            } while (train.Error > 0.01 && epoch < 1000 / neuronCount / hiddenLayerCount);
            sb.AppendLine("Epoch: " + epoch);
            sb.AppendLine("Error: " + train.Error);


            int good = 0;
            int bad = 0;
            foreach (var verf in verfData)
            {
                var output = nn.Compute(verf.Input);
                if (Enumerable.Range(0, 9).All(i => Math.Round(output[i]) == Math.Round(verf.Ideal[i])))
                    good++;
                else
                    bad++;
            }
            sb.AppendLine(string.Format("Good: {0}, Bad: {1} ", good, bad));
            var path = @"C:\Users\matte\Desktop\Results\UsingJustBinaryInputs\" + fileName + ".txt";
            if (good > 900)
                File.WriteAllText(path, "{" + sb.ToString() + "}");
        }

        private IList<IMLDataPair> GetList(IEnumerable<Tuple<int[,], Point>> input)
        {
            return input.Select(t => new BasicMLDataPair(
                new BasicMLData(t.Item1.Cast<int>().Select(i => (double)i).ToArray()),
                new BasicMLData(Enumerable.Range(0, 9).Select(i => i == t.Item2.X * 3 + t.Item2.Y ? 1.0 : 0.0).ToArray()))).Cast<IMLDataPair>().ToList();
        }

        private IList<IMLDataPair> GetList(IEnumerable<Tuple<double[], Point>> input)
        {
            return input.Select(t => new BasicMLDataPair(
                new BasicMLData(t.Item1.ToArray()),
                new BasicMLData(Enumerable.Range(0, 9).Select(i => i == t.Item2.X * 3 + t.Item2.Y ? 1.0 : 0.0).ToArray()))).Cast<IMLDataPair>().ToList();
        }

    }

    public class RecordingPlayer123EachTile : IPlayer
    {
        private IPlayer _player;
        public List<Tuple<int[,], Point>> AllMovesMade = new List<Tuple<int[,], Point>>();

        public RecordingPlayer123EachTile(IPlayer player)
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

    public class RecordingPlayer1801s : IPlayer
    {
        private IPlayer _player;
        public List<Tuple<double[], Point>> AllMovesMade = new List<Tuple<double[], Point>>();

        public RecordingPlayer1801s(IPlayer player)
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
            double[] gridState = new double[grid.Size * grid.Size * 2];
            for (int y = 0; y < grid.Size; y++)
                for (int x = 0; x < grid.Size; x++)
                    if (grid[x, y].Content == Piece.X)
                        gridState[3 * x + y] = 1;
                    else if (grid[x, y].Content == Piece.O)
                        gridState[9 + 3 * x + y] = 1;
            AllMovesMade.Add(Tuple.Create(gridState, move.Pos));
            return move;
        }


    }
}
