using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.ML;

using Chesster.Chess;
using Chesster.Logging;

namespace Chesster.ML
{
    /// <summary>
    /// Supplies a prediction engine that can be used to detect board orientation.
    /// </summary>
    public class BoardOrientationPredictionEngine : PredictionEngine<Board, BoardOrientationPrediction>
    {
        protected override string DefaultModelPath => IO.OrientationModelPath;
        protected override string DefaultTrainingDataPath => IO.BoardOrientationTrainingDataPath;

        /// <summary>
        /// Classifies the orientation of the given board. Fails if no trained model has been created or loaded first.
        /// </summary>
        /// <param name="board">The board that should be classified.</param>
        public override BoardOrientationPrediction Classify(Board board)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("No model has been trained or loaded.");

            var engine = _context.Model.CreatePredictionEngine<BoardOrientationData, BoardOrientationPrediction>(_model);
            var prediction = engine.Predict(new BoardOrientationData(board));

            Logger.Debug<BoardOrientationPredictionEngine>($"Board {board.ToFen()} was classified as: {(prediction.PredictedLabel ? "inverted" : "normal")}");

            return prediction;
        }
        /// <summary>
        /// Classifies the orientation of all given boards. Fails if no trained model has been created or loaded first.
        /// </summary>
        /// <param name="boards">The boards to classify.</param>
        public override BoardOrientationPrediction[] BulkClassify(Board[] boards)
        {
            if (!IsLoaded)
                throw new InvalidOperationException("No model has been trained or loaded.");

            IDataView classificationData = _context.Data.LoadFromEnumerable(boards.Select(board => new BoardOrientationData(board)));
            IDataView transformedData = _model.Transform(classificationData);
            BoardOrientationPrediction[] predictions = _context.Data.CreateEnumerable<BoardOrientationPrediction>(transformedData, false).ToArray();

            Logger.Debug<BoardOrientationPredictionEngine>($"{boards.Count()} boards were bulk-classified.");

            return predictions;
        }

        /// <summary>
        /// Creates a trained model from the data at the given path, and saves the model to the specified path.
        /// <para>Training data can be generated via the <see cref="Training.BoardOrientationGenerator"/>.</para>
        /// </summary>
        /// <param name="trainingDataPath">The data where the training is located at. Needs to be a appropriately-formatted CSV file.</param>
        public override PredictionEngine<Board, BoardOrientationPrediction> CreateTrainedModel(string trainingDataPath)
        {
            if (!File.Exists(trainingDataPath) || Path.GetExtension(trainingDataPath) != ".csv")
                throw new FileNotFoundException("The training data file was not found, or had an invalid extension.");

            IEstimator<ITransformer> pipeline = _context.Transforms.CopyColumns("Features", "Pieces")
                .Append(_context.Transforms.NormalizeMinMax("Features"))
                .Append(_context.BinaryClassification.Trainers.AveragedPerceptron());

            Logger.Debug<BoardOrientationPredictionEngine>("Created the estimator pipeline for board orientation prediction model.");

            _trainingDataView = _context.Data.LoadFromTextFile<BoardOrientationData>(trainingDataPath, separatorChar: ',', hasHeader: false);
            _model = pipeline.Fit(_trainingDataView);

            Logger.Debug<BoardOrientationPredictionEngine>("Board orientation prediction model was trained!");

            return this;
        }
    }
}
