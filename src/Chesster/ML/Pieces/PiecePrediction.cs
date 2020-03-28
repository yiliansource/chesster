using System.IO;
using System.Linq;

namespace Chesster.ML
{
    /// <summary>
    /// A prediction of a chess piece.
    /// </summary>
    public sealed class PiecePrediction : PieceData, IModelPrediction<string>
    {
        /// <summary>
        /// The confidence scores for the multiclass predictions.
        /// </summary>
        public float[] Score { get; set; }
        /// <summary>
        /// The predicted label of the piece, e.g. 'blackbishop'.
        /// </summary>
        public string PredictedLabel { get; set; }

        public override string ToString()
        {
            return $"Image '{Path.GetFileName(ImagePath)}' " +
                   $"predicted as '{PredictedLabel}' " +
                   $"with score '{Score.Max()}'";
        }
    }
}
