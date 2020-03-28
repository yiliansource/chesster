using System.Linq;
using System.Collections.Generic;

using Microsoft.ML.Data;

using Chesster.Chess;

namespace Chesster.ML
{
    /// <summary>
    /// Represents data that is used to train the <see cref="BoardOrientationPredictionEngine"/>
    /// </summary>
    public class BoardOrientationData : IModelData<bool>
    {
        /// <summary>
        /// A heatmap representing the pieces.
        /// </summary>
        [LoadColumn(1, 64), VectorType(64)]
        public float[] Pieces { get; set; }

        /// <summary>
        /// The label that identifies the rotation. If true, the board is inverted (black on the bottom).
        /// </summary>
        [LoadColumn(0)]
        public bool Label { get; set; }

        private static Dictionary<Piece, float> _pieceWeightMap = new Dictionary<Piece, float>()
        {
            [Piece.BlackPawn] = -0.1f,
            [Piece.BlackKnight] = -0.3f,
            [Piece.BlackBishop] = -0.35f,
            [Piece.BlackRook] = -0.5f,
            [Piece.BlackQueen] = -0.8f,
            [Piece.BlackKing] = -1f,

            [Piece.None] = 0,

            [Piece.WhitePawn] = 0.1f,
            [Piece.WhiteKnight] = 0.3f,
            [Piece.WhiteBishop] = 0.35f,
            [Piece.WhiteRook] = 0.5f,
            [Piece.WhiteQueen] = 0.8f,
            [Piece.WhiteKing] = 1f,
        };

        public BoardOrientationData() { }
        /// <summary>
        /// Creates orientation data from the given board, projecting the pieces into a heatmap.
        /// </summary>
        public BoardOrientationData(Board board)
        {
            Pieces = board.Pieces.Select(p => _pieceWeightMap[p]).ToArray();
        }
    }
}
