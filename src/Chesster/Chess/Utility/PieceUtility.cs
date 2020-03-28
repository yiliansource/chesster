namespace Chesster.Chess
{
    /// <summary>
    /// Provides utility to handle <see cref="Piece"/>s.
    /// </summary>
    public static class PieceUtility
    {
        private static Map<Piece, char> _fenCharacterLookup = new Map<Piece, char>()
        {
            [Piece.None] = ' ',

            [Piece.WhitePawn] = 'P',
            [Piece.WhiteKnight] = 'N',
            [Piece.WhiteBishop] = 'B',
            [Piece.WhiteRook] = 'R',
            [Piece.WhiteQueen] = 'Q',
            [Piece.WhiteKing] = 'K',

            [Piece.BlackPawn] = 'p',
            [Piece.BlackKnight] = 'n',
            [Piece.BlackBishop] = 'b',
            [Piece.BlackRook] = 'r',
            [Piece.BlackQueen] = 'q',
            [Piece.BlackKing] = 'k'
        };

        /// <summary>
        /// Converts the given <see cref="Piece"/> to its FEN character equivalent.
        /// </summary>
        public static char ToCharacter(this Piece p)
            => _fenCharacterLookup[p];
        /// <summary>
        /// Converts the given character to its <see cref="Piece"/> equivalent.
        /// </summary>
        public static Piece ToPiece(this char c)
            => _fenCharacterLookup[c];

        /// <summary>
        /// Checks if the given <see cref="Piece"/> is white.
        /// </summary>
        public static bool IsWhite(this Piece p)
            => (int)p >= 1 && (int)p <= 6;
        /// <summary>
        /// Checks if the given <see cref="Piece"/> is black.
        /// </summary>
        public static bool IsBlack(this Piece p)
            => (int)p >= 7 && (int)p <= 12;

        /// <summary>
        /// Transforms the given piece to the specified color.
        /// </summary>
        public static Piece TransformColor(this Piece p, bool toWhite)
        {
            return toWhite
                ? (p.IsWhite() ? p : p - 6)
                : (p.IsBlack() ? p : p + 6);
        }
    }
}
