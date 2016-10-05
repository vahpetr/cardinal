namespace Cardinal.ActivationFunctions.Contract
{
    /// <summary>
    /// Интерфейс функции активации
    /// </summary>
    public interface IActivationFunction
    {
        /// <summary>
        /// Внутренняя переменная функции.
        /// Разная функция - разное значение
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// Вычислить значения функции для массива.
        /// Алгоритм вычисления значения для массива может отличаться от вычисления значения
        /// </summary>
        /// <param name="input">Входной массив</param>
        /// <returns>Выходной массив</returns>
        double[] Compute(double[] input);

        /// <summary>
        /// Вычислить значение функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        double Compute(double x);

        /// <summary>
        /// Вычислить первую производную от функции
        /// </summary>
        /// <param name="x">Входное значение</param>
        /// <returns>Выходное значение</returns>
        double Derivative(double x);

        /// <summary>
        /// Вычислить вторую производную от функции
        /// </summary>
        /// <param name="y">Входное значение</param>
        /// <returns>Выходное значение</returns>
        double Derivative2(double y);
    }
}