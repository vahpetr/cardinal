using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;
using Cardinal.ErrorFunctions.Contratc;
using Cardinal.Layers;
using Cardinal.Layers.Contract;
using Cardinal.Networks.Contract;
using Cardinal.Neurons;
using Cardinal.Neurons.Contract;

namespace Cardinal.Networks
{
    /// <summary>
    /// Нейронная сеть
    /// </summary>
    public class Network : INetwork
    {
        /// <summary>
        /// Слои
        /// </summary>
        private readonly ILayer<INeuron>[] layers;

        private int i, l;

        /// <summary>
        /// Количество входов сети
        /// </summary>
        private int inputs;

        /// <summary>
        /// Выходной вектор сети
        /// </summary>
        private double[] output;

        /// <summary>
        /// Количество выходов сети
        /// </summary>
        private int outputs;

        /// <summary>
        /// Конструктор сети
        /// </summary>
        /// <param name="activationFunction">Функция активации</param>
        /// <param name="topology">Топология сети</param>
        public Network(IActivationFunction activationFunction, params int[] topology)
        {
            layers = new ILayer<INeuron>[topology.Length];

            layers[0] = new Layer<Neuron>(topology[0], topology[0], activationFunction);

            for (i = 1, l = topology.Length; i < l; i++)
            {
                inputs = topology[i - 1];
                layers[i] = new Layer<Neuron>(inputs, topology[i], activationFunction);
            }

            inputs = layers[0].Inputs;
            outputs = layers[layers.Length - 1].Outputs;
        }

        /// <summary>
        /// Конструктор сети
        /// </summary>
        /// <param name="layers">Слои</param>
        public Network(params ILayer<INeuron>[] layers)
        {
            // TODO вынести в сеттер
            for (i = 0, l = layers.Length - 1; i < l; i++)
            {
                if (layers[i].Outputs != layers[i + 1].Inputs) throw new ArgumentException();
            }

            inputs = layers[0].Inputs;
            outputs = layers[layers.Length - 1].Outputs;

            this.layers = layers;
        }

        /// <summary>
        /// Слои
        /// </summary>
        public ILayer<INeuron>[] Layers => layers;

        /// <summary>
        /// Количество входов сети
        /// </summary>
        public int Inputs => inputs;

        /// <summary>
        /// Количество выходов сети
        /// </summary>
        public int Outputs => outputs;

        /// <summary>
        /// Выходной вектор сети
        /// </summary>
        public double[] Output => output;

        /// <summary>
        /// Вычислить выходной сигнал
        /// </summary>
        /// <param name="input">Входной сигнал</param>
        /// <returns>Выходной сигнал</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] Compute(double[] input)
        {
            if (input.Length != inputs) throw new ArgumentException();

            for (i = 0, l = layers.Length; i < l; i++)
            {
                input = layers[i].Compute(input);
            }

            output = input;

            return input;
        }

        public IErrorFunction ErrorFunction { get; set; }
    }
}