using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class ValidMoveGetter : IValidMoveGetter
    {
        private IGameState _state;
        private IMoveFactory _moveFactory;
        private IPlayerTracker _playerTracker;

        public ValidMoveGetter(IGameState state, IMoveFactory moveFactory, IPlayerTracker playerTracker)
        {
            _state = state;
            _moveFactory = moveFactory;
            _playerTracker = playerTracker;
        }

        public IEnumerable<Move> GetMoves()
        {
            for (int x = 0; x < _state.Grid.Tiles.GetLength(0); x++)
                for (int y = 0; y < _state.Grid.Tiles.GetLength(1); y++)
                    if (_state.Grid.Tiles[x, y].Content != TileContent.Empty)
                        yield return _moveFactory.CreateMove(new Point(x, y), _playerTracker.CurrentPlayer.Piece);
        }
    }
}
