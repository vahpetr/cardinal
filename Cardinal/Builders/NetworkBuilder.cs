using System;
using System.Collections.Generic;
using Cardinal.ActivationFunctions;
using Cardinal.ActivationFunctions.Contract;
using Cardinal.Builders.Contract;
using Cardinal.ErrorFunctions;
using Cardinal.ErrorFunctions.Contratc;
using Cardinal.Layers;
using Cardinal.Layers.Contract;
using Cardinal.Lessions.Contract;
using Cardinal.Networks;
using Cardinal.Networks.Contract;
using Cardinal.Neurons;
using Cardinal.Neurons.Contract;

namespace Cardinal.Builders
{
    /// <summary>
    /// Создаёт структуру сети
    /// </summary>
    public class NetworkBuilder : IBuilder
    {
        private const double ALPHA = 6;

        /// <summary>
        /// Минимально возможное значение инициализации нейрона
        /// </summary>
        private const double MIN = -0.1d;

        /// <summary>
        /// Максимально возможное значение инициализации нейрона
        /// </summary>
        private const double MAX = 0.1d;

        // ReSharper disable once InconsistentNaming
        //private static readonly ThreadLocal<Random> rnd = new ThreadLocal<Random>(() => new Random());
        private static readonly Random rnd = new Random();
        //private static readonly ThreadLocal<Random> rnd = new ThreadLocal<Random>(() => new Random());
        private int i, l;//, j, m, k, n;
        //private ILayer<INeuron> layer;
        //private INeuron[] neurons;
        //private double min, max;
        //private double[] weights;

        // TODO функцию активации определять автоматически
        // TODO функцию ошибки перенести в обучение

        public IErrorFunction ErrorFunction { get; set; } = new SquaredErrorFunction();
        public IActivationFunction ActivationFunction { get; set; } = new BipolarSigmoidActivationFunction(ALPHA);
        public IDatasets Datasets { get; set; }
        public int[] HiddenTopology { get; set; }

        public INetwork Build()
        {
            var topology = BuildTopology();
            var layers = BuildLayers(topology);
            var network = BuildNetwork(layers);
            return network;
        }

        private INetwork BuildNetwork(ILayer<INeuron>[] layers)
        {
            return new Network(layers)
            {
                ErrorFunction = ErrorFunction
            };
        }

        private List<int> BuildTopology()
        {
            var data = Datasets[0];
            var inputs = data.Input.Length;
            var outputs = data.Desired.Length;
            var topology = new List<int>(2 + HiddenTopology.Length)
            {
                inputs
            };
            topology.AddRange(HiddenTopology);
            topology.Add(outputs);
            return topology;
        }


        // TODO разделить на две функции - для глубоких и простых сетей

        /// <summary>
        /// Построить слои сети
        /// </summary>
        /// <param name="topology">Топология</param>
        /// <returns>Топология сети</returns>
        private ILayer<INeuron>[] BuildLayers(List<int> topology)
        {
            if (topology.Count < 2) throw new ArgumentException();

            var size = topology.Count;
            var layers = new ILayer<INeuron>[size];
            var hasHiddenLayer = size > 2;

            // input layer
            if (hasHiddenLayer)
            {
                layers[0] = new Layer<Neuron>(topology[0], topology[1], ActivationFunction);
            }
            else
            {
                layers[0] = new Layer<Neuron>(topology[0], topology[0], ActivationFunction);
            }

            // hidden layer(s)
            if (hasHiddenLayer)
            {
                // функция активации скрытых слоёв(обычно используют tanh)
                var hiddenActivationFunction = new TanhActivationFunction(ALPHA);//5.99

                for (i = 1, l = size - 2; i < l; i++)
                {
                    layers[i] = new Layer<Neuron>(topology[i], topology[i + 1], hiddenActivationFunction);
                }

                layers[i++] = new Layer<Neuron>(topology[l], topology[l], hiddenActivationFunction);
            }

            // output layer

            // если у сети выходов больше одного то задача на классификацию
            var isClassification = topology[size - 1] != 1;

            // функция активации на выходном слое
            var outputActivationFunction = isClassification ? new SoftmaxActivationFunction() : ActivationFunction;

            if (hasHiddenLayer)
            {
                layers[i] = new Layer<Neuron>(topology[l], topology[size - 1], outputActivationFunction);
            }
            else
            {
                layers[++i] = new Layer<Neuron>(topology[0], topology[1], outputActivationFunction);
            }

            BuildWeights(layers);

            return layers;
        }

        private void BuildWeights(ILayer<INeuron>[] layers)
        {
            foreach (var layer in layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    neuron.Weights = new double[layer.Inputs];

                    for (i = 0, l = neuron.Weights.Length; i < l; i++)
                    {
                        neuron.Weights[i] = rnd.NextDouble() * (MAX - MIN) + MIN;
                    }

                    //neuron.Bias = 0.1;//rnd.NextDouble() * (MAX - MIN) + MIN;
                }
            }


            //max = MAX;//(Math.Sqrt(6) / Math.Sqrt(layer.Inputs + layer.Outputs));
            //min = MIN;//- max;

            //// input layer
            //layer = layers[0];
            //neurons = layer.Neurons;
            //for (j = 0, m = neurons.Length; j < m; j++)
            //{
            //    weights = neurons[j].Weights;
            //    for (k = 0, n = weights.Length; k < n; k++)
            //    {
            //        weights[k] = rnd.Value.NextDouble() * (max - min) + min;
            //    }
            //    neurons[j].Bias = rnd.Value.NextDouble() * (max + min) - min;
            //}

            //// hidden layer(s)
            //for (i = 1, l = layers.Length - 2; i < l; i++)
            //{
            //    layer = layers[i];
            //    //max = 4*(Math.Sqrt(6) / Math.Sqrt(layer.Inputs + layer.Outputs));
            //    //min = -max;
            //    neurons = layer.Neurons;
            //    for (j = 0, m = neurons.Length; j < m; j++)
            //    {
            //        weights = neurons[j].Weights;
            //        for (k = 0, n = weights.Length; k < n; k++)
            //        {
            //            weights[k] = rnd.Value.NextDouble() * (max - min) + min;
            //        }
            //        neurons[j].Bias = rnd.Value.NextDouble() * (max + min) - min;
            //    }
            //}

            //// output layer
            //layer = layers[layers.Length - 1];
            ////max = (Math.Sqrt(6) / Math.Sqrt(layer.Inputs + layer.Outputs));
            ////min = -max;

            //neurons = layer.Neurons;
            //for (j = 0, m = neurons.Length; j < m; j++)
            //{
            //    weights = neurons[j].Weights;
            //    for (k = 0, n = weights.Length; k < n; k++)
            //    {
            //        weights[k] = rnd.Value.NextDouble() * (max - min) + min;
            //    }
            //    neurons[j].Bias = rnd.Value.NextDouble() * (max + min) - min;
            //}
        }
    }
}