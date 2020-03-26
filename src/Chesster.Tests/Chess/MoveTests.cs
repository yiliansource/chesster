using System;
using System.Collections.Generic;

using Xunit;

namespace Chesster.Chess.Tests
{
    public sealed class MoveTests
    {
        [Fact]
        public void TestMoveParse()
        {
            List<(string notation, Move move)> cases = new List<(string notation, Move move)>()
            {
                { ("c2c4", new Move(new Square("c2"), new Square("c4"))) },
                { ("f3f8", new Move(new Square("f3"), new Square("f8"))) },
                { ("a1h8q", new Move(new Square("a1"), new Square("h8"), Piece.BlackQueen)) }
            };

            foreach (var (notation, move) in cases)
            {
                Assert.Equal(move, new Move(notation));
            }
        }

        [Fact]
        public void TestMoveParseException()
        {
            string[] notations = new string[] { "a1z2, c3a9", "Ea2", "Ox08" };

            foreach (string notation in notations)
            {
                Assert.Throws<FormatException>(() => new Move(notation));
            }
        }

        [Fact]
        public void TestPieceMove()
        {
            Board b = new Board();
            b[new Square("a1")] = Piece.WhiteQueen;

            Assert.Equal(Piece.None, b[new Square("h8")]);

            Move m = new Move("a1h8");
            b.Perform(m);

            Assert.Equal(Piece.WhiteQueen, b[new Square("h8")]);
            Assert.Equal(Piece.None, b[new Square("a1")]);
        }

        [Fact]
        public void TestPiecePromotion()
        {
            Board b = new Board();
            b[new Square("a7")] = Piece.WhitePawn;
            b[new Square("h2")] = Piece.BlackPawn;

            Move m = new Move("a7a8q");
            b.Perform(m);
            m = new Move("h2h1n");
            b.Perform(m);

            Assert.Equal(Piece.WhiteQueen, b[new Square("a8")]);
            Assert.Equal(Piece.BlackKnight, b[new Square("h1")]);
        }
    }
}
