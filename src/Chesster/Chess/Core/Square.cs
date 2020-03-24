using System;

namespace Chesster.Chess
{
    public struct Square
    {
        public int BoardIndex => File + Rank * 8;

        public int File { get; set; }
        public int Rank { get; set; }

        public Square(int file, int rank)
        {
            if (file < 0 || rank < 0 || file >= 8 || rank >= 8)
                throw new FormatException("Square coordinates are out of bounds [0-7].");

            File = file;
            Rank = rank;
        }
        public Square(string notation)
        {
            if (notation.Length != 2)
                throw new FormatException("Invalid notation length.");

            int file, rank;
            try
            {
                file = notation[0] - 'a';
                rank = int.Parse(notation[1].ToString()) - 1;
            }
            catch
            {
                throw new FormatException("Invalid notation format.");
            }

            if (file < 0 || rank < 0 || file >= 8 || rank >= 8)
                throw new FormatException("Square coordinates are out of bounds [0-7].");

            File = file;
            Rank = rank;
        }

        public override string ToString()
            => $"{(char)('a' + File)}{Rank + 1}";
    }
}
