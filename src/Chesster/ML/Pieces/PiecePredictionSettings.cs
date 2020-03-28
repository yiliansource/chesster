namespace Chesster
{
    /// <summary>
    /// Provides constant settings for the piece prediction pipeline.
    /// </summary>
    public static class PiecePredictionSettings
    {
        /// <summary>
        /// The width that images should be resized to.
        /// </summary>
        public const int Width = 32;
        /// <summary>
        /// The height that images should be resized to.
        /// </summary>
        public const int Height = 32;
        /// <summary>
        /// The mean value of the pixels. This is used to balance out pixels.
        /// </summary>
        public const int Mean = 117;
    }
}
