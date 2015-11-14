using SolidTacToe;
using SolidTacToe.Definitions;
using SolidTacToe.Exe;
using SolidTacToe.Moves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncogPlayground.TicTacToe
{
    public class RandomPlayer : IPlayer
    {
        private static Random rand = new Random(100);
        //private Bindings _binding;
        public RandomPlayer(Token token)
        {
            Token = token;
            //_binding = binding;
        }

        public IMove GetMove()
        {
            var move = TicTacToeBindings.Get<Move>();
            move.X = rand.Next(0, 3);
            move.Y = rand.Next(0, 3);
            move.Token = Token;
            return move;
        }

        public Token Token { get; set; }
    }
}
