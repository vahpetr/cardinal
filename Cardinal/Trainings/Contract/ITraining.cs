using Cardinal.Learning.Contract;
using Cardinal.Lessions.Contract;

namespace Cardinal.Trainings.Contract
{
    /// <summary>
    /// Тренировочный алгоритм
    /// </summary>
    public interface ITraining
    {
        /// <summary>
        /// Алгоритм обучения
        /// </summary>
        ISupervisedLearning Learning { get; }

        /// <summary>
        /// Обучающее задание
        /// </summary>
        ILession Lesson { get; }


        /// <summary>
        /// Максимум эпох
        /// </summary>
        int MaxEpoches { get; set; }

        /// <summary>
        /// Размер набора обучения
        /// </summary>
        int BatchSize { get; set; }

        /// <summary>
        /// Текущая эпоха
        /// </summary>
        int Epoch { get; }

        CopmutedError TrainError { get; set; }
        CopmutedError TestError { get; set; }
        CopmutedError ValidationError { get; set; }


        /// <summary>
        /// Минимальная ошибка
        /// </summary>
        double MinError { get; set; }

        /// <summary>
        /// Минимальная дельта 
        /// </summary>
        double MinDeltaError { get; set; }

        /// <summary>
        /// Запустить тренинг
        /// </summary>
        /// <returns>Ошибка на валидационной выборке</returns>
        void Run();
    }
}