namespace Cardinal.Lessions.Contract
{
    /// <summary>
    /// Интерфейс набора данных
    /// </summary>
    public interface IDataset
    {
        /// <summary>
        /// Вектор вопроса
        /// </summary>
        double[] Input { get; }

        /// <summary>
        /// Вектор ответа
        /// </summary>
        double[] Desired { get; }
    }
}