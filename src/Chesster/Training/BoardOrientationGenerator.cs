using System;
using System.IO;

using Chesster.ML;
using Chesster.Chess;
using Chesster.Logging;

namespace Chesster.Training
{
    /// <summary>
    /// Provides functionality to generate training data for the <see cref="ML.BoardOrientationPredictionEngine"/> using FEN positions.
    /// </summary>
    public class BoardOrientationGenerator : IDataGenerator
    {
        private readonly string _fenFile;

        /// <summary>
        /// Creates a new training data generator with FEN position data from the specified.
        /// </summary>
        public BoardOrientationGenerator(string fenFile)
        {
            if (!File.Exists(fenFile))
                throw new FileNotFoundException("The FEN input file could not be found.");

            _fenFile = fenFile;
        }

        /// <summary>
        /// Generates the training data.
        /// </summary>
        public void Generate(string outputPath)
        {
            string[] fens = File.ReadAllLines(_fenFile);

            Random rand = new Random();
            using (StreamWriter sw = new StreamWriter(File.Create(outputPath)))
            {
                foreach (string fen in fens)
                {
                    Board board = new Board(fen);

                    // Positions have a 50% chance to be inverted
                    bool rotated = rand.Next(0, 2) > 0;
                    if (rotated)
                        board = Board.Invert(board);

                    float[] pieceMap = new BoardOrientationData(board).Pieces;

                    sw.WriteLine($"{rotated},{string.Join(',', pieceMap)}");
                }
            }

            Logger.Info<BoardOrientationGenerator>($"Generated {fens.Length} board orientation training data entries.");
        }
    }
}
