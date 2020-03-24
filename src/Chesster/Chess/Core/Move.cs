using System;

namespace Chesster.Chess
{
    public struct Move
    {
        public Square Source { get; set; }
        public Square Destination { get; set; }

        public Move(Square src, Square dst)
        {
            Source = src;
            Destination = dst;
        }
        public Move(string notation)
        {
            if (notation.Length != 4)
                throw new FormatException("Invalid notation length.");

            Source = new Square(notation[0..2]);
            Destination = new Square(notation[2..4]);
        }

        public override string ToString()
            => $"{Source}{Destination}";
        public string ToString(Board context)
            => $"{context[Source].ToCharacter()}{Destination}";
    }
}
