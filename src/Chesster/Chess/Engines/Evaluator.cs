using System;
using System.Collections.Generic;
using System.Text;

namespace Chesster.Chess.Engines
{
    public static class Evaluator
    {
        private const int _defaultDepth = 20;
        
        public static Evaluation EvaluateWhite<TEngine>(Board board, int depth = _defaultDepth) where TEngine : UciEngine
            => Evaluate<TEngine>(board, true, depth);
        public static Evaluation EvaluateBlack<TEngine>(Board board, int depth = _defaultDepth) where TEngine : UciEngine
            => Evaluate<TEngine>(board, false, depth);

        private static Evaluation Evaluate<TEngine>(Board board, bool isWhite, int depth = _defaultDepth) where TEngine : UciEngine
        {
            using UciEngine engine = Activator.CreateInstance<TEngine>();
            engine.Initialize();
            return engine.Evaluate(board.ToFen(), isWhite, depth);
        }
    }
}
