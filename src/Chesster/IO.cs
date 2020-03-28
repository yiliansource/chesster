using System;
using System.IO;
using System.Collections.Generic;

namespace Chesster
{
    /// <summary>
    /// Provides static data paths for the application. Paths are always created using the executing directory.
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// The root folder where all assets are located.
        /// </summary>
        public static readonly string AssetRoot = Path.Combine(Environment.CurrentDirectory, "assets");

        /// <summary>
        /// The folder where the temporary board is extracted to during the vision process.
        /// </summary>
        public static readonly string TemporaryBoardExtractionPath = Path.Combine(AssetRoot, "tempboard");
        /// <summary>
        /// The asset folder for the spritesheets.
        /// </summary>
        public static readonly string SpritesheetsPath = Path.Combine(AssetRoot, "spritesheets");
        /// <summary>
        /// The asset folder for the backgrounds.
        /// </summary>
        public static readonly string BackgroundsPath = Path.Combine(AssetRoot, "backgrounds");

        /// <summary>
        /// The path for the training data that is used for the <see cref="ML.PiecePredictionEngine"/>.
        /// </summary>
        public static readonly string PieceTrainingDataPath = Path.Combine(AssetRoot, "training");
        /// <summary>
        /// The path for the training data that is used for the <see cref="ML.BoardOrientationPredictionEngine"/>.
        /// </summary>
        public static readonly string BoardOrientationTrainingDataPath = Path.Combine(PieceTrainingDataPath, "orientations.csv");

        /// <summary>
        /// The default path that the <see cref="ML.PiecePredictionEngine"/> will be saved to.
        /// </summary>
        public static readonly string PieceModelPath = Path.Combine(AssetRoot, "pieceModel.zip");
        /// <summary>
        /// The default path that the <see cref="ML.BoardOrientationPredictionEngine"/> will be saved to.
        /// </summary>
        public static readonly string OrientationModelPath = Path.Combine(AssetRoot, "orientationModel.zip");

        /// <summary>
        /// The path that the stockfish engine is located at.
        /// </summary>
        public static readonly string StockfishPath = Path.Combine(AssetRoot, "stockfish.exe");

        /// <summary>
        /// Returns an array of files in the specified directory.
        /// </summary>
        public static string[] GetFiles(string path)
            => Directory.GetFiles(path);
        /// <summary>
        /// Filters the files in the specified directory and returns them. Filters can be combined with '|'.
        /// </summary>
        public static string[] GetFiles(string path, string filter)
        {
            List<string> files = new List<string>();
            foreach (string filterPart in filter.Split('|'))
                foreach (string file in Directory.GetFiles(path, filterPart))
                    files.Add(file);
            return files.ToArray();
        }

        /// <summary>
        /// Combines a sequence of paths.
        /// </summary>
        public static string Combine(params string[] paths)
            => Path.Combine(paths);
    }
}
