using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;

namespace Cardinal.ActivationFunctions
{
    /// <summary>
    /// Линейно выпрямляющая функция активации.
    /// Возвращает максимум
    /// </summary>
    public class RectifiedLinearActivationFunction : IActivationFunction
    {
        /// <summary>
        /// Минимум
        /// </summary>
        private double min = 0;

        private int i, l;

        /// <summary>
        /// Конструктор линейно выпрямляющейся функции активации
        /// </summary>
        public RectifiedLinearActivationFunction()
        {
        }

        /// <summary>
        /// Конструктор линейно выпрямляющейся функции активации
        /// </summary>
        /// <param name="value">Минимум</param>
        public RectifiedLinearActivationFunction(double value)
        {
            min = value;
        }

        /// <summary>
        /// Минимум
        /// </summary>
        public double Value
        {
            get { return min; }
            set { min = value; }
        }

        /// <summary>
        /// Вычислить значения функции для массива.
        /// </summary>
        /// <param name="input">Входной массив</param>
        /// <returns>Выходной массив</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double[] Compute(double[] input)
        {
            for (i = 0, l = input.Length; i < l; i++)
            {
                input[i] = Compute(input[i]);
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
            return Math.Max(x, min);
        }

        /// <summary>
        /// Вычислить первую производную от функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative(double x)
        {
            return min;
        }

        /// <summary>
        /// Вычислить вторую производную от функции
        /// </summary>
        /// <param name="y">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative2(double y)
        {
            return min;
        }
    }
}