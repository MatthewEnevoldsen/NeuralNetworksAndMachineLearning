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
            var nn = new BasicNetwork();
            nn.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 9));//input the grid contents
            nn.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 20));
            nn.AddLayer(new BasicLayer(new ActivationSigmoid(), true, 20));
            nn.AddLayer(new BasicLayer(new ActivationSigmoid(), false, 2));//next square to put a piece (x,y)
            nn.Structure.FinalizeStructure();
            nn.Reset();

            var p1 = new RecordingPlayer(new RandomPlayer("first", Piece.X, new ValidMoveGetter()));
            var p2 = new RecordingPlayer(new FirstPlayer("second", Piece.O, new ValidMoveGetter()));

            new GameRunner().RunGames(p1, p2, 2000);

            var trainingData = GetList(p2.AllMovesMade.Take(1900));
            var verificationData = GetList(p2.AllMovesMade.Skip(1900).Take(100));

            var dataSet = new BasicMLDataSet(trainingData.Cast<IMLDataPair>().ToList());

            var train = new Backpropagation(nn, dataSet, 0.7, 0.3);
            train.BatchSize = 1;
            int epoch = 1;
            do
            {
                train.Iteration();
                epoch++;
                if (epoch % 200 == 0)
                    Console.WriteLine(train.Error);
            } while (train.Error > 0.01);

            foreach (var verf in verificationData)
            {
                var output = nn.Compute(verf.Input);
                Console.WriteLine("Output: [{0}, {1}] for expected {2} ", Math.Round(output[0]), Math.Round(output[1]), verf.Ideal);
            }
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
                new BasicMLData(t.Item1.Cast<int>().Select(i=> (double)i).ToArray()),
                new BasicMLData(new double[] { t.Item2.X, t.Item2.Y }))).Cast<IMLDataPair>().ToList();
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
                    gridState[x,y] = grid[x, y].Content.GetHashCode();
            AllMovesMade.Add(Tuple.Create(gridState, move.Pos));
            return move;
        }

        
    }
}
