namespace Cardinal.Lessions.Contract
{
    /// <summary>
    /// Интерфейс обучающего задания
    /// </summary>
    public interface ILession
    {
        /// <summary>
        /// Реперзентативные данные
        /// </summary>
        IDataset[] Dataset { set; }

        /// <summary>
        /// Обучающая выборка
        /// </summary>
        IDatasets Train { get; }

        /// <summary>
        /// Тестовая выборка
        /// </summary>
        IDatasets Test { get; }

        /// <summary>
        /// Проверчная выборка
        /// </summary>
        IDatasets Validation { get; }
    }
}