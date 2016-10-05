using System;
using System.Runtime.CompilerServices;
using Cardinal.ActivationFunctions.Contract;

namespace Cardinal.ActivationFunctions
{
    /// <summary>
    /// Биполярная сигмоидальная функция [-1...1]
    /// </summary>
    public class BipolarSigmoidActivationFunction : IActivationFunction
    {
        /// <summary>
        /// Альфа
        /// </summary>
        private double alpha = 1;//2;//5.99

        private int i, l;

        /// <summary>
        /// Конструктор биполярной сигмовидной функции
        /// </summary>
        public BipolarSigmoidActivationFunction()
        {
        }

        /// <summary>
        /// Конструктор биполярной сигмовидной функции
        /// </summary>
        /// <param name="alpha">Альфа</param>
        public BipolarSigmoidActivationFunction(double alpha)
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
            //if (-x * alpha < -45)
            //    return -1.0;

            //if (-x * alpha > 45)
            //    return 1.0;

            //if (x == 0) return 0;

            return 2d /(1d + Math.Exp(alpha*-x)) - 1d;
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
            return (1d - y*y)/ 2d;// * alpha;//нужна ли тут альфа?
        }
    }
}