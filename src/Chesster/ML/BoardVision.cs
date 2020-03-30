using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

using Chesster.Chess;
using Chesster.Logging;

namespace Chesster.ML
{
    /// <summary>
    /// Provides utility to classify a board from an image.
    /// </summary>
    public class BoardVision
    {
        /// <summary>
        /// Predicts the board at the given path.
        /// </summary>
        public static Board PredictBoard(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("An input file was not found at the given path.");

            Logger.Info<BoardVision>($"Initializing vision process for file {Path.GetFileName(imagePath)}.");
            return PredictBoard(new Bitmap(imagePath));
        }
        public static Board PredictBoard(Bitmap image)
        {
            if (image == null)
                throw new ArgumentNullException("image");

            // Search for and extract a board from the input image
            Bitmap chessboard = ExtractChessboard(image);
            if (chessboard == null)
            {
                // If no chessboard was found, return null
                return null;
            }

            // Using the board size, extract the individual squares
            Bitmap[] squares = ExtractSquares(chessboard);
            Logger.Info<BoardVision>("Board squares have been extracted!");

            string[] squarePaths = new string[64];
            for (int i = 0; i < 64; i++)
            {
                string path = IO.Combine(IO.AssetRoot, $"{'a' + (i % 8)}{7 - i / 8}.png");
                squares[i].Save(path);
                squarePaths[i] = path;
            }

            IEnumerable<PiecePrediction> piecePredictions = new PiecePredictionEngine()
                .LoadModel()
                .BulkClassify(squarePaths);

            Piece[] pieces = piecePredictions
                .Select(p => (Piece)Enum.Parse(typeof(Piece), p.PredictedLabel, true))
                .ToArray();
            Board board = new Board(pieces);

            Logger.Info<BoardVision>($"Piece prediction yielded a board with the FEN {board.ToFen()}.");

            BoardOrientationPrediction orientationPrediction = new BoardOrientationPredictionEngine()
                .LoadModel()
                .Classify(board);

            Logger.Info<BoardVision>($"Board orientation prediction yield a result of '{(orientationPrediction.PredictedLabel ? "inverted" : "normal")}'.");

            if (orientationPrediction.PredictedLabel)
            {
                board = Board.Invert(board);
                Logger.Info<BoardVision>($"New board FEN: {board.ToFen()}");
            }

            return board;
        }

        private static Bitmap ExtractChessboard(Bitmap inputImage)
        {
            BoardExtractor extractor = new BoardExtractor(inputImage);
            Rectangle? boardRect = extractor.FindChessboard(BoardExtractor.BoardOption.Largest);

            if (boardRect == null)
            {
                Logger.Info<BoardVision>("No board was found in the input image.");
                return null;
            }


            Logger.Info<BoardVision>($"Found a board ({boardRect.Value})!");

            Directory.CreateDirectory(IO.TemporaryBoardExtractionPath);
            return ExtractImageSection(inputImage, boardRect.Value);
        }
        private static Bitmap[] ExtractSquares(Bitmap chessboard)
        {
            Size squareSize = Size.Round(chessboard.Size / 8f);
            Bitmap[] pieceImages = new Bitmap[64];
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    Bitmap result = ExtractImageSection(chessboard, 
                        new Rectangle(x * squareSize.Width, y * squareSize.Height, squareSize.Width, squareSize.Height));

                    pieceImages[(7 - y) * 8 + x] = result;
                }
            return pieceImages;
        }

        private static Bitmap ExtractImageSection(Bitmap sourceImage, Rectangle sourceRect)
        {
            Bitmap extracted = new Bitmap(sourceRect.Width, sourceRect.Height);
            using (Graphics g = Graphics.FromImage(extracted))
            {
                g.DrawImage(sourceImage,
                    new Rectangle(Point.Empty, sourceRect.Size),
                    sourceRect,
                    GraphicsUnit.Pixel);
            }
            return extracted;
        }
    }
}
