using System;

namespace Chesster.Chess.Engines
{
    /// <summary>
    /// A static class to provide evaluation utility.
    /// </summary>
    public static class Evaluator
    {
        private const int _defaultDepth = 20;
        
        /// <summary>
        /// Evaluates the board from white's perspective.
        /// </summary>
        /// <typeparam name="TEngine">The type of the engine to use.</typeparam>
        /// <param name="board">The position to evaluate.</param>
        /// <param name="depth">The depth to evaluate to position to.</param>
        public static Evaluation EvaluateWhite<TEngine>(Board board, int depth = _defaultDepth) where TEngine : UciEngine
            => Evaluate<TEngine>(board, true, depth);
        /// <summary>
        /// Evaluates the board from black's perspective.
        /// </summary>
        /// <typeparam name="TEngine">The type of the engine to use.</typeparam>
        /// <param name="board">The position to evaluate.</param>
        /// <param name="depth">The depth to evaluate to position to.</param>
        public static Evaluation EvaluateBlack<TEngine>(Board board, int depth = _defaultDepth) where TEngine : UciEngine
            => Evaluate<TEngine>(board, false, depth);

        private static Evaluation Evaluate<TEngine>(Board board, bool isWhite, int depth = _defaultDepth) where TEngine : UciEngine
        {
            using UciEngine engine = Activator.CreateInstance<TEngine>();
            return engine.Evaluate(board.ToFen(), isWhite, depth);
        }
    }
}
