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
using Database;

namespace EncogPlayground
{
    public class TicTacToeBrain
    {
        const int TotalGames = 10000;
        const int TestDataCount = 9000;

        public void CanItLearnTheRules()
        {

            var p1 = new RecordingPlayer1801s(new RandomPlayer("first", Piece.X, new ValidMoveGetter()));
            var p2 = new RecordingPlayer1801s(new FirstPlayer("second", Piece.O, new ValidMoveGetter()));

            new GameRunner().RunGames(p1, p2, TotalGames);

            var inputData = GetList(p2.AllMovesMade).Take(TestDataCount).ToList();
            var verfData = GetList(p2.AllMovesMade).ToList();


            var hiddenLayerCounts = new[] { 1, 2, 3, 5, 8, 13 };
            var neuronCounts = new[] { 2, 3, 5, 8, 13, 21, 34, 55, 89, 150 };
            var actFuncs = new IActivationFunction[] { new ActivationTANH(), new ActivationSIN(), new ActivationSigmoid(), new ActivationLinear() };
            var learnRates = new[] { 0.1, 0.5, 0.7 };
            var momentums = new[] { 0.1, 0.3, 0.5 };
            var batchSizes = new[] { 1 };
            var epochs = new[] { 1, 10, 100, 1000, 5000 };//will be /nc/hlc

            var x = from hlc in hiddenLayerCounts
                    from nc in neuronCounts
                    from af in actFuncs
                    from lr in learnRates
                    from mom in momentums
                    from bat in batchSizes
                    from epoch in epochs
                    select new { hlc, nc, af, lr, mom, bat, epoch };

            x.AsParallel().ForAll(param => CanItLearnRulesWith(inputData, verfData, param.hlc, param.nc, param.af, param.lr, param.mom, param.bat, param.epoch));


        }

        private void CanItLearnRulesWith(IList<IMLDataPair> inputData, IList<IMLDataPair> verfData, int hiddenLayerCount, int neuronCount, IActivationFunction actFunc, double learnRate, double momentum, int batchSize, int maxEpochs)
        {
            var nn = CreateNetwork(inputData, hiddenLayerCount, neuronCount, actFunc);
            var train = new Backpropagation(nn, new BasicMLDataSet(inputData), learnRate, momentum);
            train.BatchSize = batchSize;
            int epoch = 1;
            do
            {
                train.Iteration();
                epoch++;
            } while (train.Error > 0.001 && epoch < maxEpochs / neuronCount / hiddenLayerCount);

            int good = verfData.Count(verf => Enumerable.Range(0, 9).All(i => Math.Round(nn.Compute(verf.Input)[i]) == Math.Round(verf.Ideal[i])));
            int bad = TotalGames - good;

            var result = new TicTacToeResult()
            {
                HiddenLayerCount = hiddenLayerCount,
                NeuronPerLayercount = neuronCount,
                ActivationFunction = actFunc.GetType().Name,
                Bad = bad,
                Good = good,
                LearningRate = learnRate,
                BatchSize = batchSize,
                Epochs = epoch,
                Error = train.Error,
                Name = "BinaryInputsAndOutputs",
            };
            var model = new DbModel();

            model.TicTacToeResult.Add(result);
            model.SaveChanges();

        }

        private static BasicNetwork CreateNetwork(IList<IMLDataPair> inputData, int hiddenLayerCount, int neuronCount, IActivationFunction actFunc)
        {
            var nn = new BasicNetwork();
            nn.AddLayer(new BasicLayer(new ActivationTANH(), false, inputData.First().Input.Count));//input the grid contents
            for (int j = 0; j < hiddenLayerCount; j++)
                nn.AddLayer(new BasicLayer(actFunc, true, neuronCount));
            nn.AddLayer(new BasicLayer(new ActivationTANH(), false, 9));//next square to put a piece 
            nn.Structure.FinalizeStructure();
            nn.Reset();
            return nn;
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
