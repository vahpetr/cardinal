namespace Cardinal.Neurons.Contract
{
    /// <summary>
    /// Интерфейс нейрона
    /// </summary>
    public interface INeuron
    {
        /// <summary>
        /// Веса
        /// </summary>
        double[] Weights { get; set; }

        /// <summary>
        /// Смещение(сигнал на дополнительном, всегда нагруженном, входе(синапсе))
        /// </summary>
        double Bias { get; set; }

        ///// <summary>
        ///// Взвешенная сумма
        ///// </summary>
        //double Sum { get; }

        /// <summary>
        /// Вычислить выходного сигнала.
        /// Скалярое произведение векторов
        /// </summary>
        /// <param name="input">Входной сигнал</param>
        /// <returns>Выходной сигнал</returns>
        double Compute(double[] input);
    }
}