using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Chesster
{
    public class PieceData
    {
        public string ImagePath;
        public string Label;

        public static IEnumerable<PieceData> LoadFromRoot(string rootFolder)
        {
            return Directory.GetDirectories(rootFolder)
                .Where(dir => Enum.TryParse(Path.GetFileName(dir), true, out Piece _))
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
