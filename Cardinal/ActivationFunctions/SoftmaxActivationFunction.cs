using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;

namespace Cardinal.ActivationFunctions
{
    /// <summary>
    /// Софтмакс функция.
    /// Применяется на выходном слое для классификаци(разбиения результатов на группы)
    /// </summary>
    public class SoftmaxActivationFunction : IActivationFunction
    {
        private int i, l;

        /// <summary>
        /// Максимальное значение на входе
        /// </summary>
        private double max = 0;

        /// <summary>
        /// Маштаб
        /// </summary>
        private double scale = 1;

        /// <summary>
        /// Конструктор софтмакс функции
        /// </summary>
        public SoftmaxActivationFunction()
        {
        }

        /// <summary>
        /// Конструктор софтмакс функции
        /// </summary>
        /// <param name="value">Маштаб</param>
        public SoftmaxActivationFunction(double value)
        {
            scale = value;
        }

        /// <summary>
        /// Маштаб
        /// </summary>
        public double Value
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Вычислить значения функции для массива.
        /// </summary>
        /// <param name="input">Входной массив</param>
        /// <returns>Выходной массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] Compute(double[] input)
        {
            max = input[0];
            for (i = 0, l = input.Length; i < l; ++i)
            {
                if (input[i] <= max) continue;

                max = input[i];
            }

            scale = 0.0;
            for (i = 0; i < l; ++i)
            {
                scale += Math.Exp(input[i] - max);
            }

            for (i = 0; i < l; ++i)
            {
                input[i] = Compute(input[i] - max);
            }

            return input;
        }

        /// <summary>
        /// Вычислить значение функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Compute(double x)
        {
            return Math.Exp(x)/scale;
        }

        /// <summary>
        /// Вычислить первую производную от функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative(double x)
        {
            return Derivative2(Compute(x));
        }

        /// <summary>
        /// Вычислить вторую производную от функции
        /// </summary>
        /// <param name="y">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative2(double y)
        {
            return (1d - y) * y;
        }
    }
}