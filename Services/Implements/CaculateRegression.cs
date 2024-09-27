using LiveChartsCore.Defaults;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI_IOT.Services.Implements
{
    public class InputData
    {
        [LoadColumn(0)]
        public float Feature { get; set; }

        [LoadColumn(1)]
        public float Label { get; set; }
    }

    public class CaculateRegression
    {
        public static (float, float) LinearRegressionFunction(ObservableCollection<ObservablePoint> datas)
        {
            var mlContext = new MLContext();

            var inputData = new List<InputData>();

            // Prepare the input data
            foreach (var data in datas)
            {
                if (data != null)
                {
                    inputData.Add(new InputData { Feature = (float)data.X, Label = (float)data.Y });
                }
            }

            // Load data into ML.NET DataView
            var trainData = mlContext.Data.LoadFromEnumerable(inputData);

            // Create a pipeline: map 'Feature' to 'Features' and then apply regression
            var pipeline = mlContext.Transforms.Concatenate("Features", "Feature")
                            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", maximumNumberOfIterations: 100));

            // Train the model
            var model = pipeline.Fit(trainData);

            // Extract the last transformer in the chain and retrieve the linear regression model
            var linearModel = model.LastTransformer.Model as Microsoft.ML.Trainers.LinearRegressionModelParameters;

            if (linearModel != null)
            {
                // Return the weight (slope) and bias (intercept) of the linear regression
                return (linearModel.Weights[0], linearModel.Bias);
            }

            throw new Exception("Model training failed or is not linear regression.");
        }
    }
}
