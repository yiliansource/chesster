namespace Chesster.Chess
{
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

        public static char PieceToCharacter(Piece p)
            => _fenCharacterLookup[p];
        public static Piece CharacterToPiece(char c)
            => _fenCharacterLookup[c];

        public static char ToCharacter(this Piece p)
            => PieceToCharacter(p);
        public static Piece ToPiece(this char c)
            => CharacterToPiece(c);

        public static bool IsWhite(this Piece p)
            => (int)p >= 1 && (int)p <= 6;
        public static bool IsBlack(this Piece p)
            => (int)p >= 7 && (int)p <= 12;

        public static Piece TransformColor(this Piece p, bool toWhite)
        {
            return toWhite
                ? (p.IsWhite() ? p : p - 6)
                : (p.IsBlack() ? p : p + 6);
        }
    }
}
