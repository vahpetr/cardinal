using Cardinal.Lessions.Contract;

namespace Cardinal.Lessions
{
    /// <summary>
    /// Данные
    /// </summary>
    public class Dataset : IDataset
    {
        /// <summary>
        /// Конструктор данных
        /// </summary>
        /// <param name="input">Входной вектор</param>
        /// <param name="desired">Ожидаемый выходной вектор</param>
        public Dataset(double[] input, double[] desired)
        {
            Input = input;
            Desired = desired;
        }

        /// <summary>
        /// Вектор вопроса
        /// </summary>
        public double[] Input { get; }

        /// <summary>
        /// Ожидаемый выходной вектор
        /// </summary>
        public double[] Desired { get; }
    }
}