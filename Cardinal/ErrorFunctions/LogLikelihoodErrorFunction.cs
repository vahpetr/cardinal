using System;
using System.Runtime.CompilerServices;
using Cardinal.ErrorFunctions.Contratc;

namespace Cardinal.ErrorFunctions
{
    /// <summary>
    /// Минимизация логарифмического правдоподобия. Самая точная
    /// </summary>
    public class LogLikelihoodErrorFunction : IErrorFunction
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
            if (output == 0) throw new ArgumentException("+ double.Epsilon ?");
            if (1 - output == 0) throw new ArgumentException("+ double.Epsilon ?");
            output = Math.Abs(output);
            return -(desired * Math.Log(output) + (1 - desired) * Math.Log(1 - output));
        }

        public double Derivaitve(double desired, double output)
        {
            if (output == 0) throw new ArgumentException("+ double.Epsilon ?");
            //поидее Math.Log(1- output) - Math.Log(output)
            output = Math.Abs(output);
            return -(desired / output - (1.0 - desired) / (1.0 - output));
        }
    }
}