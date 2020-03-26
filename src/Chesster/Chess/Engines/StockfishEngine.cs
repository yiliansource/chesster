using System;
using System.Linq;

using Chesster.Logging;

namespace Chesster.Chess.Engines
{
    public class StockfishEngine : UciEngine
    {
        public override string EnginePath => IO.StockfishPath;

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
            string ExtractNext(string key)
            {
                int keyIndex = info.IndexOf(key);
                if (keyIndex == -1)
                    return string.Empty;

                int valueIndex = keyIndex + key.Length + 1;
                int nextSpaceIndex = info.IndexOf(' ', valueIndex);
                return info[valueIndex..nextSpaceIndex];
            }
            string ExtractRunaway(string key)
            {
                int keyIndex = info.LastIndexOf(key);
                if (keyIndex == -1)
                    return string.Empty;

                int valueIndex = keyIndex + key.Length + 1;
                return info.Substring(valueIndex);
            }

            float score = 0;
            string scoreCp = ExtractNext("cp");
            if (!string.IsNullOrEmpty(scoreCp))
                score = int.Parse(scoreCp) / 100f;

            int? mate = null;
            string scoreMate = ExtractNext("mate");
            if (!string.IsNullOrEmpty(scoreMate))
                mate = int.Parse(scoreMate);

            return new Evaluation
            {
                Depth = int.Parse(ExtractNext("depth")),
                SelDepth = int.Parse(ExtractNext("seldepth")),
                MultiPv = int.Parse(ExtractNext("multipv")),
                Nodes = int.Parse(ExtractNext("nodes")),
                NodesPerSecond = int.Parse(ExtractNext("nps")),

                RelativeScore = score,
                AbsoluteScore = score * (isWhite ? 1 : -1),

                MateIn = mate,

                Moves = ExtractRunaway("pv").Split(' ').Select(s => new Move(s)).ToArray()
            };
        }
    }
}
