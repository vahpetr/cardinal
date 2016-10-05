using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;

namespace Cardinal.ActivationFunctions
{
    /// <summary>
    /// Сигмоидальная функция активации
    /// </summary>
    public class SigmoidActivationFunction : IActivationFunction
    {
        /// <summary>
        /// Альфа
        /// </summary>
        private double alpha = 1;//2;//7.61;

        private int i, l;

        /// <summary>
        /// Конструктор сигмовидной функции
        /// </summary>
        public SigmoidActivationFunction()
        {
        }

        /// <summary>
        /// Конструктор биполярной сигмовидной функции
        /// </summary>
        /// <param name="alpha">Альфа</param>
        public SigmoidActivationFunction(double alpha)
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
            //// TODO возможно должно быть не 45
            //if (-x * alpha < -45.0)
            //    return 0.0;

            //if (-x * alpha > 45.0)
            //    return 1.0;

            //if (x == 0) return 0;

            return 1d / (1d + Math.Exp(alpha * -x));
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
            return y * (1d - y);// * alpha;//нужна ли тут альфа?
        }
    }
}