using System.Collections.Generic;
using Cardinal.Lessions.Contract;
using Cardinal.Networks.Contract;

namespace Cardinal.Learning.Contract
{
    /// <summary>
    /// Интерфейс алгоритма обучения с учителем
    /// </summary>
    public interface ISupervisedLearning
    {
        /// <summary>
        /// Нейронная сеть
        /// </summary>
        INetwork Network { get; }

        /// <summary>
        /// Скорость обучения
        /// </summary>
        double LearningRate { get; set; }

        /// <summary>
        /// Бонус скорости обучения
        /// </summary>
        double LearningRateBonus { get; set; }

        /// <summary>
        /// Импульс.
        /// Используется в онлайн обучении
        /// </summary>
        double Momentum { get; set; }

        /// <summary>
        /// Регуляризация(штраф за переобучение, регрессия).
        /// Используется в оффлайн обучении
        /// </summary>
        double Regularization { get; set; }

        /// <summary>
        /// Онлайн обучение
        /// </summary>
        /// <param name="datasets">Обучающая выборка</param>
        /// <returns>Ошибка сети</returns>
        void OnlineLearn(IEnumerable<IDataset> datasets);

        /// <summary>
        /// Оффлайн обучение(пакетное)
        /// </summary>
        /// <param name="datasets">Обучающая выборка</param>
        /// <param name="batchSize">Размер обучающей выборки</param>
        /// <returns>Ошибка сети</returns>
        void OfflineLearn(IEnumerable<IDataset> datasets, int batchSize);
    }
}