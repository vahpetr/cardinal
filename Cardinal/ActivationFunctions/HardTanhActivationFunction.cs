using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;

namespace Cardinal.ActivationFunctions
{
    /// <summary>
    /// Пороговая функция
    /// </summary>
    public class HardTanhActivationFunction : IActivationFunction
    {
        private int i, l;

        /// <summary>
        /// Пороговое значение. Всё что меньше становится 0
        /// </summary>
        private double threshold = 1;

        /// <summary>
        /// Конструктор пороговой функции
        /// </summary>
        public HardTanhActivationFunction()
        {
        }

        /// <summary>
        /// Конструктор пороговой функции
        /// </summary>
        /// <param name="value">Порог</param>
        public HardTanhActivationFunction(double value)
        {
            threshold = value;
        }

        /// <summary>
        /// Порог
        /// </summary>
        public double Value
        {
            get { return threshold; }
            set { threshold = value; }
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
            if (x < -threshold) return -threshold;
            if (x > threshold) return threshold;
            return x;
        }

        /// <summary>
        /// Вычислить первую производную от функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative(double x)
        {
            return threshold;
        }

        /// <summary>
        /// Вычислить вторую производную от функции
        /// </summary>
        /// <param name="y">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative2(double y)
        {
            return threshold;
        }
    }
}