namespace Chesster.ML
{
    public interface IModelPrediction<T> : IModelData<T>
    {
        T PredictedLabel { get; set; }
    }
}
