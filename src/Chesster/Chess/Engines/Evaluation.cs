using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Chesster.Chess.Engines
{
    public struct Evaluation
    {
        public int Depth { get; set; }
        public int SelDepth { get; set; }
        public int MultiPv { get; set; }
        public int Nodes { get; set; }
        public int NodesPerSecond { get; set; }

        public float RelativeScore { get; set; }
        public float AbsoluteScore { get; set; }
        public int? MateIn { get; set; }

        public Move[] Moves { get; set; }

        public override string ToString()
            => ToString(string.Join(' ', Moves));
        public string ToString(Board context)
        {
            Board copy = new Board(context.Pieces);

            StringBuilder moves = new StringBuilder();
            int moveCounter = 0;

            foreach (Move m in Moves)
            {
                if (moveCounter % 2 == 0)
                    moves.Append((moveCounter / 2 + 1).ToString() + ". ");

                moves.Append(m.ToString(copy));
                copy.Perform(m);
                moves.Append(' ');

                moveCounter++;
            }

            moves.Remove(moves.Length - 1, 1);
            return ToString(moves.ToString());
        }

        private string ToString(string moves)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(MateIn == null ? (AbsoluteScore >= 0 ? "+" : "") + AbsoluteScore.ToString() : "#" + MateIn.Value.ToString());
            builder.Append(": ");
            builder.Append(moves);
            return builder.ToString();
        }
    }
}
