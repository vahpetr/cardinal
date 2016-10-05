namespace Cardinal.ErrorFunctions.Contratc
{
    /// <summary>
    /// Интерфейс функции ошибки
    /// </summary>
    public interface IErrorFunction
    {
        /// <summary>
        /// Вычислить. Считает ошибку
        /// </summary>
        /// <param name="desired">Желаемый вектор(t)</param>
        /// <param name="output">Выходной вектор(y)</param>
        /// <returns>Ошибка</returns>
        double Compute(double[] desired, double[] output);

        /// <summary>
        /// Вычислить. Считает ошибку
        /// </summary>
        /// <param name="desired">Желаемый сигнал(t)</param>
        /// <param name="output">Выходной сигнал(y)</param>
        /// <returns>Ошибка</returns>
        double Compute(double desired, double output);
    }
}