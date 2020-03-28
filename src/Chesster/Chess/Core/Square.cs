using System;

namespace Chesster.Chess
{
    /// <summary>
    /// Represents a square on a chessboard. Can be used to index into boards.
    /// </summary>
    public struct Square
    {
        /// <summary>
        /// The one-dimensional index on the board, starting from a1.
        /// </summary>
        public int BoardIndex => File + Rank * 8;

        /// <summary>
        /// The file of the square (x).
        /// </summary>
        public int File { get; set; }
        /// <summary>
        /// The rank of the square (y).
        /// </summary>
        public int Rank { get; set; }

        /// <summary>
        /// Creates a square with a specified file and rank.
        /// </summary>
        public Square(int file, int rank)
        {
            if (file < 0 || rank < 0 || file >= 8 || rank >= 8)
                throw new FormatException("Square coordinates are out of bounds [0-7].");

            File = file;
            Rank = rank;
        }
        /// <summary>
        /// Creates a new square from a square notation, 'g3' for example.
        /// </summary>
        /// <param name="notation"></param>
        public Square(string notation)
        {
            if (notation.Length != 2)
                throw new FormatException("Invalid notation length.");

            int file = 0, rank = 0;
            try
            {
                file = notation[0] - 'a';
                rank = int.Parse(notation[1].ToString()) - 1;
            }
            catch
            {
                throw new FormatException("Invalid notation format.");
            }
            finally
            {
                if (file < 0 || rank < 0 || file >= 8 || rank >= 8)
                    throw new FormatException("Square coordinates are out of bounds [0-7].");
            }

            File = file;
            Rank = rank;
        }

        /// <summary>
        /// Returns the formatted notation of the square.
        /// </summary>
        public override string ToString()
            => $"{(char)('a' + File)}{Rank + 1}";
    }
}
