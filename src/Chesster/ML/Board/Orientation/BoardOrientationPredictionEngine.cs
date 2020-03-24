using Microsoft.ML;

using Chesster.Chess;
using Chesster.Logging;

namespace Chesster.ML
{
    public static class BoardOrientationPredictionEngine
    {
        private static MLContext _context = new MLContext();

        public static BoardOrientationPrediction ClassifyBoard(ITransformer model, Board board)
        {
            var predictor = _context.Model.CreatePredictionEngine<BoardOrientationData, BoardOrientationPrediction>(model);

            Logger.Debug("Board orientation prediction engine created!");

            return predictor.Predict(new BoardOrientationData(board));
        }

        public static ITransformer CreateTrainedModel(string trainingDataPath, string modelOutputPath)
        {
            IEstimator<ITransformer> pipeline = _context.Transforms.CopyColumns("Features", "Pieces")
                .Append(_context.Transforms.NormalizeMinMax("Features"))
                .Append(_context.BinaryClassification.Trainers.AveragedPerceptron());

            Logger.Debug("Created the estimator pipeline for board orientation prediction model.");

            IDataView trainingDataView = _context.Data.LoadFromTextFile<BoardOrientationData>(trainingDataPath, separatorChar: ',', hasHeader: false);
            ITransformer model = pipeline.Fit(trainingDataView);

            Logger.Debug("Board orientation prediction model was trained!");

            _context.Model.Save(model, trainingDataView.Schema, modelOutputPath);

            Logger.Debug($"Trained model was saved to {modelOutputPath}.");

            return model;
        }

        public static ITransformer LoadModel(string modelPath)
        {
            return _context.Model.Load(modelPath, out _);
        }
    }
}
