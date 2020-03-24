using System;
using System.IO;
using System.Collections.Generic;

namespace Chesster
{
    public static class IO
    {
        public static readonly string AssetRoot = Path.Combine(Environment.CurrentDirectory, "assets");

        public static readonly string TemporaryBoardExtractionPath = Path.Combine(AssetRoot, "tempboard");
        public static readonly string SpritesheetsPath = Path.Combine(AssetRoot, "spritesheets");
        public static readonly string BackgroundsPath = Path.Combine(AssetRoot, "backgrounds");
        public static readonly string TrainingDataPath = Path.Combine(AssetRoot, "training");

        public static readonly string PieceModelPath = Path.Combine(AssetRoot, "pieceModel.zip");
        public static readonly string OrientationModelPath = Path.Combine(AssetRoot, "orientationModel.zip");

        public static string[] GetFiles(string path)
            => Directory.GetFiles(path);
        public static string[] GetFiles(string path, string filter)
        {
            List<string> files = new List<string>();
            foreach (string filterPart in filter.Split('|'))
                foreach (string file in Directory.GetFiles(path, filterPart))
                    files.Add(file);
            return files.ToArray();
        }

        public static string Combine(params string[] paths)
            => Path.Combine(paths);
    }
}
