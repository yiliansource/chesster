using System;
using System.Text;

namespace Chesster.Chess
{
    public struct Move
    {
        public Square Source { get; set; }
        public Square Destination { get; set; }
        public Piece Promotion { get; set; }

        public Move(Square src, Square dst) : this(src, dst, Piece.None) { }
        public Move(Square src, Square dst, Piece promotion)
        {
            Source = src;
            Destination = dst;
            Promotion = promotion;
        }
        public Move(string notation)
        {
            if (notation.Length < 4 && notation.Length > 5)
                throw new FormatException("Invalid notation length.");

            Source = new Square(notation[0..2]);
            Destination = new Square(notation[2..4]);

            Promotion = notation.Length == 5 ? notation[4].ToPiece() : Piece.None;
        }

        public override string ToString()
            => $"{Source}{Destination}{(Promotion != Piece.None ? Promotion.ToCharacter().ToString() : "")}";
        public string ToString(Board context)
        {
            StringBuilder builder = new StringBuilder();

            Piece piece = context[Source];
            bool isPawn = piece == Piece.WhitePawn || piece == Piece.BlackPawn;
            bool isCapture = context[Destination] != Piece.None;

            if (!isPawn)
                builder.Append(char.ToUpper(piece.ToCharacter()));
            if (isPawn && isCapture)
                builder.Append(Source.ToString()[0..1]);
            if (isCapture)
                builder.Append('x');

            builder.Append(Destination);

            if (Promotion != Piece.None)
                builder.Append("=" + char.ToUpper(Promotion.ToCharacter()));

            return builder.ToString();
        }
    }
}
