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
    }
}
