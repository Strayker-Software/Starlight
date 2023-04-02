using Microsoft.ML;
using Microsoft.ML.Data;
using Starlight.Domain.Models;
using Starlight.Infrastructure.IO;
using Starlight.Infrastructure.IO.Enums;
using Starlight.Infrastructure.IO.Interfaces;
using Starlight.Service.MLModels;
using static Microsoft.ML.DataOperationsCatalog;

namespace Starlight.Service.Services.Classificators
{
    internal class BinaryClassificator
    {
        private MLContext _mlContext;
        private ITransformer _model;
        private readonly string _datasetName;
        private IDataView? _dataView;
        private IOutput _output;

        public BinaryClassificator(
            string datasetName,
            string datasetPath,
            bool hasHeader,
            IOSystem io,
            bool debug = false)
        {
            switch (io)
            {
                case IOSystem.Console:
                    _output = new ConsoleIO();
                    break;

                default:
                    _output = new ConsoleIO();
                    break;
            }

            if (debug)
                _output.WriteLine("------------- Building " + datasetName + " Dataset Object ------------");
            _mlContext = new MLContext();
            _datasetName = datasetName;

            if (datasetPath == null)
                datasetPath = Path.Combine(Environment.CurrentDirectory, "Dataset", _datasetName + ".txt");
            else
                datasetPath = Path.Combine(datasetPath, _datasetName + ".txt");

            TrainTestData splitDataView = LoadData(datasetPath, hasHeader);

            _model = BuildAndTrainModel(splitDataView.TrainSet);

            Evaluate(splitDataView.TestSet, debug);
            if (debug)
                _output.WriteLine("----------------------------------------------------\n");
        }

        public BinaryClassificator(string datasetName, string datasetPath)
            : this(datasetName, datasetPath, false, io: IOSystem.Console)
        {
        }

        public Intent Classify(string query, bool debug = false)
        {
            var statement = new ClassificationData
            {
                Content = query
            };

            return PredictSingleItem(statement, debug);
        }

        private ITransformer BuildAndTrainModel(IDataView splitTrainSet)
        {
            var estimator = _mlContext.Transforms.Text
                .FeaturizeText(
                    outputColumnName: "Features",
                    inputColumnName: nameof(ClassificationData.Content))
                .Append(_mlContext.BinaryClassification.Trainers
                    .SdcaLogisticRegression(
                        labelColumnName: "Label",
                        featureColumnName: "Features"));

            return estimator.Fit(splitTrainSet);
        }

        private void Evaluate(IDataView splitTestSet, bool debug = false)
        {
            IDataView predictions = _model.Transform(splitTestSet);

            try
            {
                CalibratedBinaryClassificationMetrics metrics = _mlContext.BinaryClassification
                    .Evaluate(predictions, "Label");

                if (debug)
                {
                    _output.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
                    _output.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
                    _output.WriteLine($"F1Score: {metrics.F1Score:P2}");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                if (debug)
                    _output.WriteLine("Test fraction percentage too low to evaluate.");
            }
        }

        private TrainTestData LoadData(string datasetPath, bool hasHeader)
        {
            _dataView = _mlContext.Data.LoadFromTextFile<ClassificationData>(datasetPath, hasHeader: hasHeader);
            // testFraction -> Percentage of phrases compared | Default: 10%
            TrainTestData splitDataView = _mlContext.Data.TrainTestSplit(_dataView, testFraction: 0.00000001);
            return splitDataView;
        }

        private Intent PredictSingleItem(ClassificationData statement, bool debug = false)
        {
            PredictionEngine<ClassificationData, ClassificationPrediction> predictionFunction =
                _mlContext.Model.CreatePredictionEngine<ClassificationData, ClassificationPrediction>(_model);

            var resultprediction = predictionFunction.Predict(statement);

            Intent intent = new Intent();
            intent.Name = _datasetName;
            intent.Score = resultprediction.Probability;

            if (debug)
            {
                _output.WriteLine("-------- Prediction of " + _datasetName + " model --------");
                _output.WriteLine("Query: " + resultprediction.Content
                    + " | Prediction (" + _datasetName + "): " + Convert.ToBoolean(resultprediction.Prediction)
                    + " | Probability: " + resultprediction.Probability);

                _output.WriteLine("----------------------------------------------------");
                _output.WriteLine("\n");
            }

            return intent;
        }
    }
}