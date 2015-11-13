using System.Reflection;
using Ninject;
using Ninject.Modules;
using SolidTacToe.Definitions;
using SolidTacToe.Factories;
using SolidTacToe.Moves;
using System;
using Ninject.Extensions.NamedScope;
using EncogPlayground.TicTacToe;

namespace SolidTacToe.Exe
{
    /// <summary>
    /// Contains our IOC bindings that composes the pieces of our application.
    /// </summary>
    public class TicTacToeBindings : NinjectModule
    {
        private static readonly IKernel Container;

        static TicTacToeBindings()
        {
            Container = new StandardKernel();
            Container.Load(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// Get an implementation of the specified interface
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>()
        {
            return Container.Get<T>();
        }

        /// <summary>
        /// Defines interface -> concrete mappings. Where the magic happens.
        /// </summary>
        public override void Load()
        {
            const int gridSize = 3;
            Bind<IMoveTracker>()
                .To<MoveTracker>()
                .InParentScope()
                .WithPropertyValue("PlayerOne", new RandomPlayer(Token.X))
                .WithPropertyValue("PlayerTwo", new RandomPlayer(Token.O));

            Bind<IMove>().To<Move>();
            Bind<IMoveValidator>().To<MoveValidator>();
            Bind<Token>().ToMethod(x => Get<IMoveTracker>().GetCurrentPlayer().Token);
            Bind<IGridFactory>().To<EmptyGridFactory>();

            Bind<IGrid>()
                .ToMethod(x => Get<IGridFactory>().Create<Grid>(gridSize))
                .InParentScope()
                .WithPropertyValue("Size", x => gridSize);

            Bind<IGameConditionsFactory>()
                .To<StandardGameConditionsFactory>();

            Bind<IMoveProvider>().To<ValidMoveProvider>();

            Bind<IGameRunner>()
                .To<GameRunner>()
                .InSingletonScope()
                .WithPropertyValue("MoveProvider", Get<IMoveProvider>())
                .WithPropertyValue("GameOverConditions", Get<IGameConditionsFactory>().Create());
        }
    }



}