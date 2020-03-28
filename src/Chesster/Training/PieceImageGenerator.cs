using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;

using Chesster.Logging;

namespace Chesster.Training
{
    /// <summary>
    /// Generates training data for <see cref="Piece"/>s.
    /// </summary>
    public class PieceImageGenerator : IDataGenerator
    {
        /// <summary>
        /// The number of files that will be generated. Read-only, this value is dependant on the number of supplied spritesheets and backgrounds.
        /// </summary>
        public int ResultingFileCount => Enum.GetValues(typeof(Piece)).Length * Spritesheets.Length * Backgrounds.Length;

        /// <summary>
        /// The size at which the images should be generated.
        /// </summary>
        public int Size { get; }
        /// <summary>
        /// The spritesheet files that should be used during generation.
        /// </summary>
        public string[] Spritesheets { get; set; }
        /// <summary>
        /// The background files that should be used during generation.
        /// </summary>
        public string[] Backgrounds { get; set; }

        /// <summary>
        /// Creates a new training data generator for <see cref="Piece"/>s.
        /// </summary>
        /// <param name="size">The size at which the images should be generated.</param>
        /// <param name="spritesheets">The spritesheet files that should be used during generation.</param>
        /// <param name="backgrounds"></param>
        public PieceImageGenerator(int size, string[] spritesheets, string[] backgrounds)
        {
            if (size <= 0)
                throw new ArgumentException("The size can not be zero or negative.");
            if (spritesheets == null || backgrounds == null || spritesheets.Length + backgrounds.Length < 2)
                throw new ArgumentException("A minimum of one spritesheet and one background needs to be provided to the generator.");

            Size = size;
            Spritesheets = spritesheets;
            Backgrounds = backgrounds;
        }

        /// <summary>
        /// Generates the piece images.
        /// </summary>
        public void Generate(string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath))
                throw new ArgumentException("The output path can not be null or empty.");

            Piece[] pieces = (Piece[])Enum.GetValues(typeof(Piece));
            Bitmap[] backgrounds = Backgrounds.Select(bg => new Bitmap(bg)).ToArray();
            Bitmap[] spritesheets = Spritesheets.Select(ss => new Bitmap(ss)).ToArray();

            Dictionary<Piece, int> variantIndices = pieces.ToDictionary(p => p, p => 0);

            // Ensure output directories exist.
            foreach (Piece p in pieces)
                Directory.CreateDirectory(Path.Combine(outputPath, p.ToString().ToLower()));

            // Create permutations between spritesheets and backgrounds
            foreach (Bitmap spritesheet in spritesheets)
            {
                if (spritesheet.Height < Size)
                    Logger.Warn<PieceImageGenerator>("The spritesheet size is smaller than the desired output size. Images will be scaled up.");

                foreach (Bitmap background in backgrounds)
                {
                    int xIndex = 0;
                    foreach (Piece p in pieces)
                    {
                        Bitmap variant = new Bitmap(Size, Size);
                        using (var graphics = Graphics.FromImage(variant))
                        {
                            // Draw the background ...
                            graphics.DrawImage(background,
                                new Rectangle(0, 0, Size, Size),
                                new Rectangle(0, 0, background.Height, background.Height),
                                GraphicsUnit.Pixel);
                            // ... and the current piece over it.
                            graphics.DrawImage(spritesheet,
                                new Rectangle(0, 0, Size, Size),
                                new Rectangle(xIndex * spritesheet.Height, 0, spritesheet.Height, spritesheet.Height), 
                                GraphicsUnit.Pixel);
                        }
                        string variantName = $"{p.ToString().ToLower()}_{variantIndices[p]:000}.png";
                        string variantPath = Path.Combine(outputPath, p.ToString().ToLower(), variantName);

                        variant.Save(variantPath, ImageFormat.Png);

                        xIndex++;
                        variantIndices[p]++;
                    }
                }
            }

            Logger.Info<PieceImageGenerator>($"Generated {ResultingFileCount} piece images.");
        }
    }
}
