using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;

namespace Cardinal.ActivationFunctions
{
    /// <summary>
    /// Функция гиперболического тангенса
    /// </summary>
    public class TanhActivationFunction : IActivationFunction
    {
        /// <summary>
        /// Альфа
        /// </summary>
        private double alpha = 1;

        private int i, l;

        /// <summary>
        /// Конструктор функции гиперболического тангенса
        /// </summary>
        public TanhActivationFunction()
        {
        }

        /// <summary>
        /// Конструктор функции гиперболического тангенса
        /// </summary>
        /// <param name="alpha">Альфа</param>
        public TanhActivationFunction(double alpha)
        {
            this.alpha = alpha;
        }

        /// <summary>
        /// Альфа
        /// </summary>
        public double Value
        {
            get { return alpha; }
            set { alpha = value; }
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
            // корректно работает на интервале до 30, обычно ограничивают 20, реже 10
            // так как у нас данные нормализованы они вообще за 1 выходить не должны
            //// по идее нужно домножать на альфу
            //if (alpha * x < -20.0)
            //    return -1.0;

            //if (alpha * x > 20.0)
            //    return 1.0;

            //if (x == 0) return 0;

            return Math.Tanh(alpha * x);
        }

        /// <summary>
        /// Вычислить первую производную от функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Derivative(double x)
        {
            //if (x == 0) return 0;
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
            //if (x == 0) return 0;
            // тоже самое что и (1 - y) * (1 + y)
            return 1d - y*y;// * alpha; //нужна ли тут альфа?
        }
    }
}