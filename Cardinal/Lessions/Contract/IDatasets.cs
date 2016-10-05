using System.Collections.Generic;

namespace Cardinal.Lessions.Contract
{
    /// <summary>
    /// Интерфейс наборов данных
    /// </summary>
    public interface IDatasets : IEnumerable<IDataset>//, IEnumerator<IDataset>
    {
        IDataset this[int i] { get; }

        IDataset[] Values { get; set; }

        /// <summary>
        /// Получить обучающий набор
        /// </summary>
        /// <param name="size">Размер</param>
        /// <returns>Обучающий набор</returns>
        IEnumerable<IDataset> Get(int size);

        /// <summary>
        /// Перемешать данные
        /// </summary>
        void Shuffle();

        /// <summary>
        /// Проверяет есть ли данные
        /// </summary>
        /// <returns>Логичесткое значение</returns>
        bool HasValues();
    }
}