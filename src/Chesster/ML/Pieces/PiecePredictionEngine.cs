using System.Linq;
using System.Collections.Generic;

using Microsoft.ML;

using Chesster.Logging;

namespace Chesster.ML
{
    public static class PiecePredictionEngine
    {
        private static MLContext _context = new MLContext();

        public static PiecePrediction ClassifyPiece(ITransformer model, string imagePath)
        {
            var predictor = _context.Model.CreatePredictionEngine<PieceData, PiecePrediction>(model);
            Logger.Debug("Piece prediction engine created!");
            return predictor.Predict(new PieceData { ImagePath = imagePath });
        }
        public static IEnumerable<PiecePrediction> ClassifyPieces(ITransformer model, string[] imagePaths)
        {
            IDataView testData = _context.Data.LoadFromEnumerable(imagePaths.Select(path => new PieceData { ImagePath = path }));
            IDataView predictions = model.Transform(testData);

            Logger.Debug($"{imagePaths.Length} piece images were bulk-classified.");

            return _context.Data.CreateEnumerable<PiecePrediction>(predictions, true);
        }

        public static ITransformer CreateTrainedModel(string trainingDataPath, string modelOutputPath)
        {
            IEstimator<ITransformer> pipeline = _context.Transforms
                // Load the images from their data paths into the 'input' data column
                .LoadImages(outputColumnName: "input", imageFolder: "images", inputColumnName: nameof(PieceData.ImagePath))

                // Append a procedure to resize, grayscale and extract the images
                .Append(_context.Transforms.ResizeImages("input", PiecePredictionSettings.Width, PiecePredictionSettings.Height))
                .Append(_context.Transforms.ConvertToGrayscale("input"))
                .Append(_context.Transforms.ExtractPixels("input", interleavePixelColors: true, offsetImage: PiecePredictionSettings.Mean))

                // Add mappings (label -> key, prediction -> prediction), and append the classification trainer
                .Append(_context.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: nameof(PieceData.Label)))
                .Append(_context.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "input"))
                .Append(_context.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel"));

            Logger.Debug("Created the estimator pipeline for piece prediction model.");

            IEnumerable<PieceData> trainingData = PieceData.LoadFromRoot(trainingDataPath);
            IDataView trainingDataView = _context.Data.LoadFromEnumerable(trainingData);
            ITransformer model = pipeline.Fit(trainingDataView);

            Logger.Debug("Piece prediction model was trained!");

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
