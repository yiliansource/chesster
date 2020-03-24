using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Chesster.ML
{
    public class BoardExtractor
    {
        public enum BoardOption
        {
            Largest,
            Smallest
        }

        public float MaxColorDelta { get; set; } = 0.1f;
        public float SegmentRatioTolerance { get; set; } = 0.05f;
        public int MinTileSize { get; set; } = 4;
        public bool AdaptColorDuringScan { get; set; } = true;

        private readonly string _imagePath;

        public BoardExtractor(string imagePath)
        {
            if (!File.Exists(imagePath))
                throw new FileNotFoundException("An image does not exist at the given path.");

            _imagePath = imagePath;
        }

        public Rectangle? FindChessboard()
            => FindChessboard(BoardOption.Largest);
        public Rectangle? FindChessboard(BoardOption option)
        {
            Rectangle[] chessboards = FindChessboards();
            if (chessboards.Length == 0)
                return null;

            IOrderedEnumerable<Rectangle> sizeSortedChessboards = chessboards.OrderByDescending(b => b.Width + b.Height);
            return option switch
            {
                BoardOption.Largest => sizeSortedChessboards.First(),
                BoardOption.Smallest => sizeSortedChessboards.Last(),
                _ => throw new ArgumentException("Invalid board option."),
            };
        }

        public Rectangle[] FindChessboards()
        {
            // TODO: Document, comment and optimize!
            
            List<Rectangle> rectangles = new List<Rectangle>();

            using (Bitmap bmp = new Bitmap(_imagePath))
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    for (int x = 0; x < bmp.Width; x++)
                    {
                        if (rectangles.Any(r => r.Contains(x, y)))
                            continue;

                        int segmentLength = 0;
                        int cumulativeWidth = 0;
                        int cumulativeHeight = 0;

                        int startX = x;
                        int startY = y;

                        bool xSuccess = false;

                        for (int i = 0; i < 8; i++)
                        {
                            if (startX + segmentLength > bmp.Width)
                            {
                                //Console.WriteLine($"Aborting scan after {i} iteration(s); out of image bounds (x).");
                                break;
                            }

                            int length = ScanSegmentX(bmp, startX, y, MaxColorDelta, AdaptColorDuringScan);
                            startX += length;
                            cumulativeWidth += length;

                            if (i == 0)
                            {
                                segmentLength = length;
                                if (length < MinTileSize)
                                {
                                    //Console.WriteLine($"Aborting scan after {i} iteration(s); tile size too small ({length}).");
                                    break;
                                }
                            }
                            else
                            {
                                if (Math.Abs(1 - ((float)length / segmentLength)) > SegmentRatioTolerance)
                                {
                                    //Console.WriteLine($"Aborting scan after {i} iteration(s); invalid x segment length ({length}).");
                                    break;
                                }

                                if (i == 7)
                                {
                                    xSuccess = true;
                                }
                            }
                        }

                        if (xSuccess)
                        {
                            //Console.WriteLine($"Found success during x scan with length {segmentLength}");

                            for (int i = 0; i < 8; i++)
                            {
                                if (startY + segmentLength > bmp.Height)
                                {
                                    //Console.WriteLine($"Aborting scan after {i} iteration(s); out of image bounds (y).");
                                    break;
                                }

                                int length = ScanSegmentY(bmp, x, startY, MaxColorDelta, AdaptColorDuringScan);
                                startY += length;
                                cumulativeHeight += length;

                                if (Math.Abs(1 - ((float)length / segmentLength)) > SegmentRatioTolerance)
                                {
                                    //Console.WriteLine($"Aborting scan after {i} iteration(s); invalid y segment length ({length}).");
                                    break;
                                }

                                if (i == 7)
                                {
                                    // SUCCESS!
                                    //Console.WriteLine("Found success during y scan aswell!");

                                    rectangles.Add(new Rectangle(x, y, cumulativeWidth, cumulativeHeight));
                                }
                            }

                            x += 8 * segmentLength;
                        }
                        else
                        {
                            x += segmentLength;
                        }
                        x--;
                    }
                }
            }

            return rectangles.ToArray();
        }

        private static int ScanSegmentX(Bitmap bmp, int x, int y, float maxDelta, bool adaptColor)
            => ScanSegment(bmp, x, y, 1, 0, maxDelta, adaptColor);
        private static int ScanSegmentY(Bitmap bmp, int x, int y, float maxDelta, bool adaptColor)
            => ScanSegment(bmp, x, y, 0, 1, maxDelta, adaptColor);

        private static int ScanSegment(Bitmap bmp, int x, int y, int dx, int dy, float maxDelta, bool adaptColor)
        {
            if (dx == 0 && dy == 0)
                throw new ArgumentException("A direction has to be assigned to the scan instruction via either the dx or the dy variable.");

            Color origin = bmp.GetPixel(x, y);

            for (int i = 0; x + i < bmp.Width && y + i < bmp.Height; i++)
            {
                Color scan = bmp.GetPixel(x + i * dx, y + i * dy);
                float delta = CalculateColorDelta(origin, scan);

                if (delta > maxDelta)
                    return i;

                if (adaptColor)
                    origin = scan;
            }

            if (dx > dy) return bmp.Width - x;
            else return bmp.Height - y;
        }


        private static float CalculateColorDelta(Color a, Color b)
        {
            float normalizedSumA = (a.R + a.G + a.B) / (255f * 3);
            float normalizedSumB = (b.R + b.G + b.B) / (255f * 3);
            return Math.Abs(normalizedSumB - normalizedSumA);
        }
    }
}
