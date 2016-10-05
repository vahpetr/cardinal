using System.Linq;
using Cardinal.Lessions.Contract;
using Cardinal.Networks.Contract;
using Cardinal.Trainings;
using Cardinal.Trainings.Contract;
using Xunit.Abstractions;

namespace Cardinal.Tests.Utilites
{
    public class NetworkOutput
    {
        private readonly ITestOutputHelper output;

        public NetworkOutput(ITestOutputHelper output)
        {
            this.output = output;
        }

        public void PrintNetworkWeights(INetwork network)
        {
            for (int i = 0, l = network.Layers.Length; i < l; i++)
            {
                var layer = network.Layers[i];
                output.WriteLine("layer: " + i);
                for (int j = 0, k = layer.Neurons.Length; j < k; j++)
                {
                    var neuron = layer.Neurons[j];
                    output.WriteLine(neuron.Weights.Aggregate("weights:", (s, w) => s + (", " + w), s => s + ", bias: " + neuron.Bias));
                }
            }
        }

        private void PrintNetworkDetails(INetwork network)
        {
            for (int i = 0, l = network.Layers.Length; i < l; i++)
            {
                var layer = network.Layers[i];
                output.WriteLine("layer: " + i);
                for (int j = 0, k = layer.Neurons.Length; j < k; j++)
                {
                    var neuron = layer.Neurons[j];
                    output.WriteLine(neuron.Weights.Aggregate("weights:",
                        (s, w) => s + (", " + w),
                        s => s + ", bias: " + neuron.Bias + ", output: " + layer.Output[j])); //+ ", sum: " + neuron.Sum +
                }
            }
        }

        private void PrintValidationResult(ITraining training)
        {
            var network = training.Learning.Network;

            output.WriteLine("Validation:");
            var datasets = training.Lesson.Train;

            PrintTreningResult(network, datasets);
        }

        private void PrintTreningResult(INetwork network, IDatasets datasets)
        {
            // TODO пока так но вообще нужно кешировать результат network.Compute и разделять на группы, а не OrderByDescending
            foreach (var dataset in datasets.OrderByDescending(p => network.Compute(p.Input)[0]))
            {
                output.WriteLine($"{string.Join(",", dataset.Input)} -   {network.Compute(dataset.Input)[0]}");
            }
        }

        private void PrintTreningDetails(CopmutedError error)
        {
            output.WriteLine($"Error Value: {error.Value}");
            output.WriteLine($"Error Accuracy: {error.Accuracy}");
            output.WriteLine($"Error Delta: {error.Delta}");
        }

        private void PrintTreningDetails(ITraining training)
        {
            output.WriteLine($"Epoch: {training.Epoch}");
            PrintTreningDetails(training.ValidationError);
        }

        public void PrintDetails(ITraining training)
        {
            output.WriteLine("Winner:");
            PrintNetworkDetails(training.Learning.Network);
            PrintValidationResult(training);
            PrintTreningDetails(training);
        }
    }
}