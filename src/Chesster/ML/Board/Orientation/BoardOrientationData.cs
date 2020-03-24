using System.Linq;

using Microsoft.ML.Data;

using Chesster.Chess;

namespace Chesster.ML
{
    public class BoardOrientationData
    {
        [LoadColumn(1, 64), VectorType(64)]
        public float[] Pieces;

        [LoadColumn(0)]
        public bool Label;

        private static Map<Piece, float> _pieceWeightMap = new Map<Piece, float>()
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
        public BoardOrientationData(Board board)
        {
            Pieces = board.Pieces.Select(p => _pieceWeightMap[p]).ToArray();
        }
    }
}
