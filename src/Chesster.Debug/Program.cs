using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Chesster.ML;
using Chesster.Chess;
using Chesster.Training;

using Chesster.Logging;

namespace Chesster
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Logger.RegisterOutput<ConsoleLogOutput>();
            Logger.RegisterOutput<FileLogOutput>();
            Logger.Settings.MinimumLevel = LogLevel.Debug;

            new PieceImageGenerator(64, IO.TrainingDataPath, IO.GetFiles(IO.SpritesheetsPath, "*.png|*.jpg"), IO.GetFiles(IO.BackgroundsPath, "*.png|*.jpg")).Generate();
            new BoardOrientationGenerator(IO.Combine(IO.AssetRoot, "fens.txt")).Generate();
            
            PiecePredictionEngine.CreateTrainedModel(IO.TrainingDataPath, IO.PieceModelPath);
            BoardOrientationPredictionEngine.CreateTrainedModel(IO.Combine(IO.TrainingDataPath, "orientations.csv"), IO.OrientationModelPath);

            BoardVision.PredictBoard(@"D:\YilianSource\chesster\chesster-core\verifying\example04.png");

            Logger.Dispose();
            Console.ReadKey();
        }
    }
}