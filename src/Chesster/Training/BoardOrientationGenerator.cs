using System;
using System.IO;

using Chesster.ML;
using Chesster.Chess;
using Chesster.Logging;

namespace Chesster.Training
{
    public class BoardOrientationGenerator : IDataGenerator
    {
        private readonly string _fenFile;

        public BoardOrientationGenerator(string fenFile)
        {
            _fenFile = fenFile;
        }

        public void Generate()
        {
            string[] fens = File.ReadAllLines(_fenFile);

            Directory.CreateDirectory(IO.TrainingDataPath);

            Random rand = new Random();
            using (StreamWriter sw = new StreamWriter(File.Create(IO.Combine(IO.TrainingDataPath, "orientations.csv"))))
            {
                foreach (string fen in fens)
                {
                    Board board = new Board(fen);

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
