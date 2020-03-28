using System.Text;

namespace Chesster.Chess.Engines
{
    /// <summary>
    /// Contains the evaluation of a chess engine.
    /// </summary>
    public struct Evaluation
    {
        /// <summary>
        /// The depth of the evaluation.
        /// </summary>
        public int Depth { get; set; }
        /// <summary>
        /// The maximum depth reached by a PV node.
        /// </summary>
        public int SelDepth { get; set; }

        public int MultiPv { get; set; }
        /// <summary>
        /// The amount of nodes searched.
        /// </summary>
        public int Nodes { get; set; }
        /// <summary>
        /// The average amount of nodes searched per seconds.
        /// </summary>
        public int NodesPerSecond { get; set; }

        /// <summary>
        /// The score of the evaluation. Positive values are good for white, negative good for black and 0 denotes an equal position.
        /// </summary>
        public float Score { get; set; }
        /// <summary>
        /// If a mate was found, this denotes in how many moves it can be achieved.
        /// </summary>
        public int? MateIn { get; set; }

        /// <summary>
        /// The best line suggested by the evaluation.
        /// </summary>
        public Move[] Moves { get; set; }

        /// <summary>
        /// Returns the evaluated score together with the best line.
        /// </summary>
        public override string ToString()
            => ToString(string.Join(' ', Moves));
        /// <summary>
        /// Returns the evaluated score together with the best line, formatted to the context of a board.
        /// </summary>
        public string ToString(Board context)
        {
            // Make sure to copy the board, so all moves can be performed without destroying the context.
            Board copy = new Board(context.Pieces);

            StringBuilder moves = new StringBuilder();

            // Count the moves, to allow the insertion of the move number when its white's turn.
            int moveCounter = -1;

            foreach (Move m in Moves)
            {
                // Only append the score if it is white to move.
                if (++moveCounter % 2 == 0)
                    moves.Append((moveCounter / 2 + 1).ToString() + ". ");

                // Append the move formatted using the context.
                moves.Append(m.ToString(copy));
                // Perform the move, so the pieces are at the correct positions.
                copy.Perform(m);

                moves.Append(' ');
            }

            // Make sure to remove the trailing space
            moves.Remove(moves.Length - 1, 1);

            return ToString(moves.ToString());
        }

        private string ToString(string moves)
        {
            StringBuilder builder = new StringBuilder();
            // Append the evaluation score, or the mating score, if available.
            builder.Append(MateIn == null ? (Score >= 0 ? "+" : "") + Score.ToString() : "#" + MateIn.Value.ToString());
            builder.Append(": ");
            builder.Append(moves);
            return builder.ToString();
        }
    }
}
