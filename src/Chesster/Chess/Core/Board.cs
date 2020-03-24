using System;
using System.Linq;
using System.Collections.Generic;

namespace Chesster.Chess
{
    public class Board
    {
        public static Board Empty => new Board();
        public static Board Default => new Board(FenUtility.DefaultPosition);

        public IReadOnlyCollection<Piece> Pieces => _pieces;

        private Piece[] _pieces;

        public Piece this[Square sq]
        {
            get => _pieces[sq.BoardIndex];
            set => _pieces[sq.BoardIndex] = value;
        }

        public Board() : this(new Piece[64]) { }
        public Board(Piece[] pieces)
        {
            if (pieces.Length != 64)
                throw new ArgumentException("The chessboard needs to contain 64 pieces.");

            _pieces = pieces;
        }
        public Board(string fen) : this(FenUtility.FenToPieces(fen)) { }

        public void Perform(Move m)
        {
            this[m.Destination] = this[m.Source];
            this[m.Source] = Piece.None;
        }

        public string ToFen()
            => FenUtility.PiecesToFen(_pieces);
        public override string ToString()
            => ToFen();

        public static Board Invert(Board b)
            => new Board(b._pieces.Reverse().ToArray());
    }
}
