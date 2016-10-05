using System;
using System.Runtime.CompilerServices;
using Cardinal.Neurons.Contract;

namespace Cardinal.Neurons
{
    /// <summary>
    /// Нейрон
    /// </summary>
    public class Neuron : INeuron
    {
        /// <summary>
        /// Смещение(сигнал на дополнительном, всегда нагруженном, входе(синапсе))
        /// </summary>
        private double bias = 0.1;

        private int i, l;

        /// <summary>
        /// Взвешенная сумма
        /// </summary>
        private double sum;

        /// <summary>
        /// Веса
        /// </summary>
        private double[] weights;

        /// <summary>
        /// Конструктор нейрона
        /// </summary>
        public Neuron()
        {
        }

        /// <summary>
        /// Конструктор нейрона
        /// </summary>
        /// <param name="weights">Веса</param>
        /// <param name="bias">Порог</param>
        public Neuron(double[] weights, double bias)
        {
            if (weights.Length <= 0) throw new ArgumentException();

            Weights = weights;
            Bias = bias;
        }

        /// <summary>
        /// Веса
        /// </summary>
        public double[] Weights
        {
            get { return weights; }
            set
            {
                if (value.Length <= 0) throw new ArgumentException();
                weights = value;
            }
        }

        /// <summary>
        /// Смещение(сигнал на дополнительном, всегда нагруженном, входе(синапсе))
        /// </summary>
        public double Bias
        {
            get { return bias; }
            set { bias = value; }
        }

        ///// <summary>
        ///// Взвешенная сумма
        ///// </summary>
        //public double Sum => sum;

        /// <summary>
        /// Вычислить выходного сигнала.
        /// Скалярое произведение векторов
        /// </summary>
        /// <param name="input">Входной сигнал</param>
        /// <returns>Выходной сигнал</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Compute(double[] input)
        {
            for (sum = 0.0, i = 0, l = input.Length; i < l; i++)
            {
                sum += input[i]*weights[i];
            }

            sum += bias;

            return sum;
        }
    }
}