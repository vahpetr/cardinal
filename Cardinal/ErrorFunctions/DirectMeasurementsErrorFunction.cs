using System;
using System.Runtime.CompilerServices;
using Cardinal.ErrorFunctions.Contratc;

namespace Cardinal.ErrorFunctions
{
    /// <summary>
    /// Прямое измерение.
    /// Возможно такого метода нет
    /// </summary>
    public class DirectMeasurementsErrorFunction : IErrorFunction
    {
        private double sum;
        private int i, n;

        /// <summary>
        /// Вычислить. Считает ошибку
        /// </summary>
        /// <param name="desired">Желаемый вектор(t)</param>
        /// <param name="output">Выходной вектор(y)</param>
        /// <returns>Ошибка</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Compute(double[] desired, double[] output)
        {
            sum = 0;
            for (i = 0, n = desired.Length; i < n; i++)
            {
                sum += Compute(desired[i], output[i]);
            }
            return sum;
        }

        /// <summary>
        /// Вычислить. Считает ошибку
        /// </summary>
        /// <param name="desired">Желаемый сигнал(t)</param>
        /// <param name="output">Выходной сигнал(y)</param>
        /// <returns>Ошибка</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Compute(double desired, double output)
        {
            return Math.Sqrt(desired * desired + output * output);
        }
    }
}