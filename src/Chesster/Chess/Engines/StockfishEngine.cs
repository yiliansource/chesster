using System;
using System.Linq;

using Chesster.Logging;

namespace Chesster.Chess.Engines
{
    /// <summary>
    /// A wrapper for the Stockfish engine.
    /// </summary>
    public class StockfishEngine : UciEngine
    {
        protected override string EnginePath => IO.StockfishPath;

        public override Evaluation Evaluate(string fen, bool isWhite, int depth)
        {
            SendCommand($"ucinewgame");
            SendCommand($"position fen {fen} {(isWhite ? 'w' : 'b')} - - 0 1");
            SendCommand($"go depth {depth}");

            Logger.Debug<StockfishEngine>($"Sent {fen} with color {(isWhite ? "white" : "black")} to Stockfish!");

            string result = ReadResult("bestmove");
            string info = result.Split("\r\n", StringSplitOptions.RemoveEmptyEntries)[^2];

            Logger.Debug<StockfishEngine>($"Received Stockfish response: {info}");

            return EvaluateInfoString(info, isWhite);
        }

        private Evaluation EvaluateInfoString(string info, bool isWhite)
        {
            // Helper method; extracts the next value after the specified key
            string ExtractNext(string key)
            {
                int keyIndex = info.IndexOf(key);
                if (keyIndex == -1)
                    return string.Empty;

                int valueIndex = keyIndex + key.Length + 1;
                int nextSpaceIndex = info.IndexOf(' ', valueIndex);
                return info[valueIndex..nextSpaceIndex];
            }
            // Helper method; extracts everything after the specified key
            string ExtractRunaway(string key)
            {
                int keyIndex = info.LastIndexOf(key);
                if (keyIndex == -1)
                    return string.Empty;

                int valueIndex = keyIndex + key.Length + 1;
                return info.Substring(valueIndex);
            }

            float score = 0;
            int? mate = null;

            // Stockfish either returns a score based to centipawns or mating.
            // Make sure to extract the given one.
            switch (ExtractNext("score"))
            {
                case "cp": score = int.Parse(ExtractNext("cp")) / 100f; break;
                case "mate": mate = int.Parse(ExtractNext("mate")); break;
            }

            // Stockfish passes score relative to the side that has the move.
            // We want an absolute score, so we negate the value if black has the move.
            if (!isWhite)
                score *= -1;

            return new Evaluation
            {
                Depth = int.Parse(ExtractNext("depth")),
                SelDepth = int.Parse(ExtractNext("seldepth")),
                MultiPv = int.Parse(ExtractNext("multipv")),
                Nodes = int.Parse(ExtractNext("nodes")),
                NodesPerSecond = int.Parse(ExtractNext("nps")),

                Score = score,
                MateIn = mate,

                Moves = ExtractRunaway("pv").Split(' ').Select(s => new Move(s)).ToArray()
            };
        }
    }
}
