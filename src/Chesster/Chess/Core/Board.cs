using System;
using System.Linq;

namespace Chesster.Chess
{
    public class Board
    {
        public static Board Empty => new Board();
        public static Board Default => new Board(FenUtility.DefaultPosition);

        public Piece[] Pieces { get; }

        public Piece this[Square sq]
        {
            get => Pieces[sq.BoardIndex];
            set => Pieces[sq.BoardIndex] = value;
        }

        public Board() : this(new Piece[64]) { }
        public Board(Piece[] pieces)
        {
            if (pieces.Length != 64)
                throw new ArgumentException("The chessboard needs to contain 64 pieces.");

            Pieces = (Piece[])pieces.Clone();
        }
        public Board(string fen) : this(FenUtility.FenToPieces(fen)) { }

        public void Perform(Move m)
        {
            this[m.Destination] = this[m.Source];
            this[m.Source] = Piece.None;

            if (m.Promotion != Piece.None)
                this[m.Destination] = m.Promotion.TransformColor(this[m.Destination].IsWhite());
        }

        public string ToFen()
            => FenUtility.PiecesToFen(Pieces);
        public override string ToString()
            => ToFen();

        public static Board Invert(Board b)
            => new Board(b.Pieces.Reverse().ToArray());
    }
}
