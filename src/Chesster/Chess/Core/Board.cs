using System;

namespace Chesster.Chess
{
    /// <summary>
    /// Provides a chessboard representation.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// An empty chessboard.
        /// </summary>
        public static Board Empty => new Board();
        /// <summary>
        /// A chessboard with the default pieces set up.
        /// </summary>
        public static Board Default => new Board(FenUtility.DefaultPosition);

        /// <summary>
        /// A collection of all the squares on the board.
        /// </summary>
        public Piece[] Pieces { get; }

        /// <summary>
        /// Gets or sets the piece at the given square.
        /// </summary>
        public Piece this[Square sq]
        {
            get => Pieces[sq.BoardIndex];
            set => Pieces[sq.BoardIndex] = value;
        }

        /// <summary>
        /// Creates an empty board.
        /// </summary>
        public Board() : this(new Piece[64]) { }
        /// <summary>
        /// Creates a board using the specified pieces.
        /// </summary>
        /// <param name="pieces">The collection of pieces. Must be 64 long.</param>
        public Board(Piece[] pieces)
        {
            if (pieces.Length != 64)
                throw new ArgumentException("The chessboard needs to contain 64 pieces.");

            // Create a copy of the array, to allow modification of the pieces array without modifying the original.
            Pieces = (Piece[])pieces.Clone();
        }
        /// <summary>
        /// Creates a board using the specified FEN notation.
        /// </summary>
        public Board(string fen) : this(FenUtility.FenToPieces(fen)) { }

        /// <summary>
        /// Performs a given move on the board.
        /// </summary>
        public void Perform(Move m)
        {
            // Move the piece at the source to the destination and leave an empty square behind.
            this[m.Destination] = this[m.Source];
            this[m.Source] = Piece.None;

            // If the move is a promotion, make sure to promote the piece to the correct piece color.
            if (m.Promotion != Piece.None)
                this[m.Destination] = m.Promotion.TransformColor(this[m.Destination].IsWhite());
        }

        /// <summary>
        /// Returns the FEN representation of the board.
        /// </summary>
        public string ToFen()
            => FenUtility.PiecesToFen(Pieces);
        /// <summary>
        /// Returns the FEN representation of the board.
        /// </summary>
        public override string ToString()
            => ToFen();

        /// <summary>
        /// Inverts the given board, essentially rotating it by 180°.
        /// </summary>
        public static Board Invert(Board b)
        {
            Array invertedPieceArray = (Array)b.Pieces.Clone();
            Array.Reverse(invertedPieceArray);
            return new Board((Piece[])invertedPieceArray);
        }
    }
}
