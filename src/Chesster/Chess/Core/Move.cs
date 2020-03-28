using System;
using System.Text;

namespace Chesster.Chess
{
    /// <summary>
    /// Represents a move on a <see cref="Board"/>.
    /// </summary>
    public struct Move
    {
        /// <summary>
        /// The source square to move from.
        /// </summary>
        public Square Source { get; set; }
        /// <summary>
        /// The destination square to move to.
        /// </summary>
        public Square Destination { get; set; }
        /// <summary>
        /// The piece to promote to after the move. <see cref="Piece.None"/> if the move is not a promotion.
        /// </summary>
        public Piece Promotion { get; set; }

        /// <summary>
        /// Creates a move from a source to a destination square.
        /// </summary>
        public Move(Square src, Square dst) : this(src, dst, Piece.None) { }
        /// <summary>
        /// Creates a promotion move from a source to a destination square.
        /// </summary>
        public Move(Square src, Square dst, Piece promotion)
        {
            Source = src;
            Destination = dst;
            Promotion = promotion;
        }
        /// <summary>
        /// Creates a move from a long notation. Supports promotions. Example: a2a4, f7f8q
        /// </summary>
        public Move(string notation)
        {
            // Only allow notations in long format
            if (notation.Length < 4 && notation.Length > 5)
                throw new FormatException("Invalid notation length.");

            // Extract the source and destination
            Source = new Square(notation[0..2]);
            Destination = new Square(notation[2..4]);

            // If the notation contains enough characters, extract the promotion piece
            Promotion = notation.Length == 5 ? char.ToLower(notation[4]).ToPiece() : Piece.None;
        }

        /// <summary>
        /// Returns the long notation representation of the move. Example: a2a4, f7f8q
        /// </summary>
        public override string ToString()
            => $"{Source}{Destination}{(Promotion != Piece.None ? Promotion.ToCharacter().ToString() : "")}";
        /// <summary>
        /// Returns the short notation representation of the move. Example: a3, Nxc7, b8=Q
        /// </summary>
        /// <param name="context">The board to provide the piece context.</param>
        public string ToString(Board context)
        {
            // NOTE: This does not take ambiguous moves into account.
            // If two pieces can move to the same square the method will not take this into consideration.

            StringBuilder builder = new StringBuilder();

            Piece piece = context[Source];
            bool isPawn = piece == Piece.WhitePawn || piece == Piece.BlackPawn;
            bool isCapture = context[Destination] != Piece.None;

            // Pawns have no symbol in short notation
            if (!isPawn)
                builder.Append(char.ToUpper(piece.ToCharacter()));
            // If a pawn captures a piece, make sure to include the file
            if (isPawn && isCapture)
                builder.Append(Source.ToString()[0]);
            // Captures are annoted with a small 'x'
            if (isCapture)
                builder.Append('x');

            builder.Append(Destination);

            // If a promotion occured, make sure to include the promotion identifier (=) and the character.
            if (Promotion != Piece.None)
                builder.Append("=" + char.ToUpper(Promotion.ToCharacter()));

            return builder.ToString();
        }
    }
}
