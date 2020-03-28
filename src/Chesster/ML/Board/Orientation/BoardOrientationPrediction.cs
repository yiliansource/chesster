namespace Chesster.ML
{
    /// <summary>
    /// A prediction of the orientation of a board.
    /// </summary>
    public sealed class BoardOrientationPrediction : BoardOrientationData, IModelPrediction<bool>
    {
        /// <summary>
        /// The confidence score of the prediction.
        /// </summary>
        public float Score { get; set; }
        /// <summary>
        /// The actual prediction. True means that the board is most likely inverted (black on the bottom).
        /// </summary>
        public bool PredictedLabel { get; set; }
    }
}
