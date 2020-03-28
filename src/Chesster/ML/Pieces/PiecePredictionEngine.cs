using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.ML;

using Chesster.Logging;

namespace Chesster.ML
{
    /// <summary>
    /// Supplies a prediction engine that can be used to detect pieces from images.
    /// </summary>
    public class PiecePredictionEngine : PredictionEngine<string, PiecePrediction>
    {
        protected override string DefaultModelPath => IO.PieceModelPath;
        protected override string DefaultTrainingDataPath => IO.PieceTrainingDataPath;

        /// <summary>
        /// Classifies the image at the given path and returns a prediction. Fails if no trained model has been created or loaded first.
        /// </summary>
        /// <param name="imagePath">The path of the image.</param>
        public override PiecePrediction Classify(string imagePath)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("No model has been trained or loaded.");

            Microsoft.ML.PredictionEngine<PieceData, PiecePrediction> engine = _context.Model.CreatePredictionEngine<PieceData, PiecePrediction>(_model);
            PiecePrediction prediction = engine.Predict(new PieceData { ImagePath = imagePath });

            Logger.Debug<PiecePredictionEngine>($"Image {Path.GetFileName(imagePath)} was classified as: {prediction.PredictedLabel}");

            return prediction;
        }

        /// <summary>
        /// Bulk-classifies a sequence of images and returns corresponding predictions. Fails if no trained model has been created or loaded first.
        /// </summary>
        /// <param name="imagePaths">The paths of the images. The order of the elements will be retained in the returned predictions.</param>
        public override PiecePrediction[] BulkClassify(string[] imagePaths)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("No model has been trained or loaded.");

            IDataView classificationData = _context.Data.LoadFromEnumerable(imagePaths.Select(path => new PieceData { ImagePath = path }));
            IDataView transformedData = _model.Transform(classificationData); 
            PiecePrediction[] predictions = _context.Data.CreateEnumerable<PiecePrediction>(transformedData, false).ToArray();

            Logger.Debug<PiecePredictionEngine>($"{imagePaths.Count()} piece images were bulk-classified.");

            return predictions;
        }

        /// <summary>
        /// Creates a trained model from existing specified training data.
        /// Training data can be generated via the <see cref="Training.PieceImageGenerator"/>.
        /// </summary>
        /// <param name="trainingDataPath">The directory where the training data is located.</param>
        public override PredictionEngine<string, PiecePrediction> CreateTrainedModel(string trainingDataPath)
        {
            if (!Directory.Exists(trainingDataPath))
                throw new DirectoryNotFoundException("The specified training data directory was not found.");

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

            Logger.Debug<PiecePredictionEngine>("Created the estimator pipeline for piece prediction model.");

            IEnumerable<PieceData> trainingData = PieceData.LoadFromRoot(trainingDataPath);

            _trainingDataView = _context.Data.LoadFromEnumerable(trainingData);
            _model = pipeline.Fit(_trainingDataView);

            Logger.Debug<PiecePredictionEngine>("Piece prediction model was trained!");

            return this;
        }
    }
}
