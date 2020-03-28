using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Chesster.ML
{
    /// <summary>
    /// Represents data that is used to train the <see cref="PiecePredictionEngine"/>
    /// </summary>
    public class PieceData : IModelData<string>
    {
        /// <summary>
        /// The path of the image.
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// The piece label of the image, e.g. 'whiteknight'.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Yields all images from a root folder, using a directory-key -> image strategy.
        /// </summary>
        public static IEnumerable<PieceData> LoadFromRoot(string rootFolder)
        {
            return Directory.GetDirectories(rootFolder)
                // Find all folders that are named after a piece
                .Where(dir => Enum.TryParse(Path.GetFileName(dir), true, out Piece _))
                // Project the files to piece datas, using the folder name as the identifying key
                .SelectMany(dir =>
                {
                    Piece piece = Enum.Parse<Piece>(Path.GetFileName(dir), true);
                    return Directory.GetFiles(dir)
                        .Select(file => new PieceData
                        {
                            ImagePath = file,
                            Label = piece.ToString().ToLower()
                        });
                });
        }
    }
}
