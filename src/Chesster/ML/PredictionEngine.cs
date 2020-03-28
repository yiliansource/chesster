using System;
using System.Collections.Generic;

using Microsoft.ML;

using Chesster.Logging;

namespace Chesster.ML
{
    /// <summary>
    /// Represents the base for a machine-learning-based prediction engine.
    /// </summary>
    /// <typeparam name="TInput">The type that input objects should have.</typeparam>
    /// <typeparam name="TOutput">The type that output objects should have.</typeparam>
    public abstract class PredictionEngine<TInput, TOutput>
    {
        /// <summary>
        /// Was a model loaded or created and trained?
        /// </summary>
        public bool IsLoaded => _model != null;
        /// <summary>
        /// Was a model created and trained?
        /// </summary>
        public bool IsTrained => _model != null && _trainingDataView != null;

        /// <summary>
        /// The path where the default model is located at. Parameterless overrides will default to this path.
        /// </summary>
        protected abstract string DefaultModelPath { get; }
        /// <summary>
        /// The path where the default training data is located at. Parameterless overrides will default to this path.
        /// </summary>
        protected abstract string DefaultTrainingDataPath { get; }

        protected static readonly MLContext _context = new MLContext();

        protected IDataView _trainingDataView;
        protected ITransformer _model;

        /// <summary>
        /// Classifies the given input and returns the corresponding output.
        /// </summary>
        public abstract TOutput Classify(TInput input);
        /// <summary>
        /// Bulk-classifies the given input array and returns the corresponding output array.
        /// </summary>
        public abstract TOutput[] BulkClassify(TInput[] input);

        /// <summary>
        /// Creates a trained model based on the training data at the default path.
        /// </summary>
        public PredictionEngine<TInput, TOutput> CreateTrainedModel()
            => CreateTrainedModel(DefaultTrainingDataPath);
        /// <summary>
        /// Creates a trained model based on the training data at the specified path.
        /// </summary>
        public abstract PredictionEngine<TInput, TOutput> CreateTrainedModel(string trainingDataPath);

        /// <summary>
        /// Loads a trained model from the default path.
        /// </summary>
        public PredictionEngine<TInput, TOutput> LoadModel()
            => LoadModel(DefaultModelPath);
        /// <summary>
        /// Loads a trained model from the specified path.
        /// </summary>
        public virtual PredictionEngine<TInput, TOutput> LoadModel(string path)
        {
            _model = _context.Model.Load(path, out _);

            Logger.Debug<PredictionEngine<TInput, TOutput>>($"Trained model was loaded from {path}.");

            return this;
        }

        /// <summary>
        /// Saves the model to the default path. Throws an exception if the model was not previously trained.
        /// </summary>
        public PredictionEngine<TInput, TOutput> SaveModel()
            => SaveModel(DefaultModelPath);
        /// <summary>
        /// Saves the model to the specified path. Throws an exception if the model was not previously trained.
        /// </summary>
        public virtual PredictionEngine<TInput, TOutput> SaveModel(string path)
        {
            if (!IsTrained)
                throw new InvalidOperationException("The model cannot be saved, since it hasn't been previously trained.");

            _context.Model.Save(_model, _trainingDataView.Schema, path);

            Logger.Debug<PredictionEngine<TInput, TOutput>>($"Trained model was saved to {path}.");

            return this;
        }
    }
}
