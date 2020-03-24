using System;

using Xunit;

namespace Chesster.Chess.Tests
{
    public sealed class BoardTests
    {
        [Fact]
        public void TestBoardInversion()
        {
            Board board = new Board();
            Square a1 = new Square("a1");
            Square h8 = new Square("h8");
            board[a1] = Piece.WhiteKing;
            board[h8] = Piece.BlackKing;

            board = Board.Invert(board);
            Assert.Equal(Piece.WhiteKing, board[h8]);
            Assert.Equal(Piece.BlackKing, board[a1]);
        }
    }
}