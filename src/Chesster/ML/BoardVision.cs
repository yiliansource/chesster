using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

using System.Drawing;

using Chesster.Chess;
using Chesster.Logging;

namespace Chesster.ML
{
    public class BoardVision
    {

        public static Board PredictBoard(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("An input file was not found at the given path.");

            string tempboardPath = IO.Combine(IO.TemporaryBoardExtractionPath, "tempboard.png");
            if (!ExtractChessboard(imagePath, tempboardPath, out Size boardSize))
                return null;

            string[] pieceImagePaths = ExtractSquares(tempboardPath, IO.TemporaryBoardExtractionPath, Size.Round(boardSize / 8f));

            Logger.Info<BoardVision>("Board squares have been extracted!");

            Microsoft.ML.ITransformer model;

            model = PiecePredictionEngine.LoadModel(IO.PieceModelPath);
            var piecePredictions = PiecePredictionEngine.ClassifyPieces(model, pieceImagePaths);

            Piece[] pieces = piecePredictions.Select(p => (Piece)Enum.Parse(typeof(Piece), p.PredictedLabel, true)).ToArray();
            Board board = new Board(pieces);

            Logger.Info<BoardVision>($"Piece prediction yielded a board with the FEN {board.ToFen()}.");

            model = BoardOrientationPredictionEngine.LoadModel(IO.OrientationModelPath);
            var orientationPrediction = BoardOrientationPredictionEngine.ClassifyBoard(model, board);

            Logger.Info<BoardVision>($"Board orientation prediction yield a result of '{(orientationPrediction.PredictedLabel ? "inverted" : "normal")}'.");

            if (orientationPrediction.PredictedLabel)
            {
                board = Board.Invert(board);
                Logger.Info<BoardVision>($"New board FEN: {board.ToFen()}");
            }

            return board;
        }

        private static bool ExtractChessboard(string input, string output, out Size boardSize)
        {
            BoardExtractor extractor = new BoardExtractor(input);
            Rectangle? boardRect = extractor.FindChessboard(BoardExtractor.BoardOption.Largest);

            if (boardRect == null)
            {
                Logger.Info<BoardVision>("No board was found in the input image.");

                boardSize = Size.Empty;
                return false;
            }


            Logger.Info<BoardVision>($"Found a board ({boardRect.Value})!");

            Directory.CreateDirectory(IO.TemporaryBoardExtractionPath);
            ExtractImageSection(input, output, boardRect.Value);

            boardSize = boardRect.Value.Size;
            return true;
        }
        private static string[] ExtractSquares(string input, string outputDir, Size squareSize)
        {
            string[] pieceImagePaths = new string[64];
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    string imagePath = IO.Combine(outputDir, $"{new Square(x, 7 - y)}.png");
                    ExtractImageSection(input, imagePath, new Rectangle(x * squareSize.Width, y * squareSize.Height, squareSize.Width, squareSize.Height));
                    pieceImagePaths[(7 - y) * 8 + x] = imagePath;
                }
            return pieceImagePaths;
        }

        private static void ExtractImageSection(string sourceImage, string destImage, Rectangle sourceRect)
        {
            Bitmap extracted = new Bitmap(sourceRect.Width, sourceRect.Height);
            using (Graphics g = Graphics.FromImage(extracted))
            {
                g.DrawImage(Image.FromFile(sourceImage),
                    new Rectangle(Point.Empty, sourceRect.Size),
                    sourceRect,
                    GraphicsUnit.Pixel);
            }
            extracted.Save(destImage);
        }
    }
}
