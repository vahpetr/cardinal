using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;
using Cardinal.Layers.Contract;
using Cardinal.Neurons.Contract;

namespace Cardinal.Layers
{
    /// <summary>
    /// Слой нейронной сети
    /// </summary>
    /// <typeparam name="TNeuron"></typeparam>
    public class Layer<TNeuron> : ILayer<TNeuron>
        where TNeuron : INeuron, new()
    {
        /// <summary>
        /// Нейроны слоя
        /// </summary>
        private readonly TNeuron[] neurons;

        /// <summary>
        /// Выходной вектор сигнала слоя
        /// </summary>
        private readonly double[] output;

        /// <summary>
        /// Функция активации
        /// </summary>
        private IActivationFunction activationFunction;

        private int i, l;

        /// <summary>
        /// Количество входов слоя
        /// </summary>
        private int inputs;

        /// <summary>
        /// Количество выходов слоя
        /// </summary>
        private int outputs;

        /// <summary>
        /// Конструктор слоя
        /// </summary>
        /// <param name="inputs">Количество входов - количество весов нейронов</param>
        /// <param name="outputs">Количество выходов - количество нейронов в слое</param>
        /// <param name="activationFunction">Функция активации</param>
        public Layer(int inputs, int outputs, IActivationFunction activationFunction)
        {
            if (inputs <= 0) throw new ArgumentException();
            if (outputs <= 0) throw new ArgumentException();
            if (activationFunction == null) throw new ArgumentNullException();

            this.inputs = inputs;
            this.outputs = outputs;
            this.activationFunction = activationFunction;

            neurons = new TNeuron[outputs];

            for (i = 0; i < outputs; i++)
            {
                neurons[i] = new TNeuron();
            }

            output = new double[outputs];
        }

        /// <summary>
        /// Конструктор слоя
        /// </summary>
        /// <param name="neurons">Массив нейронов</param>
        public Layer(TNeuron[] neurons)
        {
            if (neurons.Length <= 0) throw new ArgumentException();
            if (neurons[0].Weights.Length <= 0) throw new ArgumentException();

            //TODO добавить проверку что у всех нейронов равное количеств весов
            inputs = neurons[0].Weights.Length;
            outputs = neurons.Length;

            this.neurons = neurons;

            //sum = new double[outputs];
            output = new double[outputs];
        }

        /// <summary>
        /// Количество выходов слоя
        /// </summary>
        public int Outputs => outputs;

        /// <summary>
        /// Функция активации
        /// </summary>
        public IActivationFunction ActivationFunction
        {
            get { return activationFunction; }
            set { activationFunction = value; }
        }

        /// <summary>
        /// Количество входов слоя
        /// </summary>
        public int Inputs => inputs;

        /// <summary>
        /// Выходной вектор сигнала слоя
        /// </summary>
        public double[] Output => output;

        /// <summary>
        /// Нейроны слоя
        /// </summary>
        public TNeuron[] Neurons => neurons;

        /// <summary>
        /// Вычислить выходной сигнал
        /// </summary>
        /// <param name="input">Входной сигнал</param>
        /// <returns>Выходной сигнал</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] Compute(double[] input)
        {
            for (i = 0, l = neurons.Length; i < l; i++)
            {
                // Вычисляем сумму
                output[i] = neurons[i].Compute(input);
            }

            //обновляем output и возвращаем его
            return activationFunction.Compute(output);
        }
    }
}