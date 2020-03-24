using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;

namespace Chesster.Chess.Tests
{
    public sealed class FenTests
    {
        private static readonly string[] _fenPositions = new string[]
        {
            "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"
        };
        private static readonly Piece[][] _piecePositions = new Piece[][]
        {
            new Piece[64]
            {
                Piece.WhiteRook, Piece.WhiteKnight, Piece.WhiteBishop, Piece.WhiteQueen, Piece.WhiteKing, Piece.WhiteBishop, Piece.WhiteKnight, Piece.WhiteRook,
                Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn, Piece.WhitePawn,
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None,
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None,
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None,
                Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None, Piece.None,
                Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn, Piece.BlackPawn,
                Piece.BlackRook, Piece.BlackKnight, Piece.BlackBishop, Piece.BlackQueen, Piece.BlackKing, Piece.BlackBishop, Piece.BlackKnight, Piece.BlackRook
            }
        };

        private static IEnumerable<(string fen, Piece[] pieces)> BuildTestScenarios()
        {
            int maximumScenarios = Math.Max(_fenPositions.Length, _piecePositions.Length);
            for (int i = 0; i < maximumScenarios; i++)
            {
                yield return (_fenPositions[i], _piecePositions[i]);
            }
        }

        [Fact]
        public void TestFenToPieces()
        {
            foreach (var (fen, pieces) in BuildTestScenarios())
            {
                Assert.Equal(pieces, FenUtility.FenToPieces(fen));
            }
        }

        [Fact]
        public void TestPiecesToFen()
        {
            foreach (var (fen, pieces) in BuildTestScenarios())
            {
                Assert.Equal(fen, FenUtility.PiecesToFen(pieces));
            }
        }

        [Fact]
        public void TestCyclicInvariance()
        {
            foreach (var (fen, _) in BuildTestScenarios())
            {
                Assert.Equal(fen, FenUtility.PiecesToFen(FenUtility.FenToPieces(fen)));
            }
        }

        [Fact]
        public void TestFenException()
        {
            Assert.Throws<FormatException>(() => FenUtility.FenToPieces("This is not a fen position."));
        }
    }
}
