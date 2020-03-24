using System.IO;
using System.Linq;

namespace Chesster
{
    public class PiecePrediction : PieceData
    {
        public float[] Score;

        public string PredictedLabel;

        public override string ToString()
        {
            return $"Image '{Path.GetFileName(ImagePath)}' " +
                   $"predicted as '{PredictedLabel}' " +
                   $"with score '{Score.Max()}'";
        }
    }
}
