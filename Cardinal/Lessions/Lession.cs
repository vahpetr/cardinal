using System;
using Cardinal.Lessions.Contract;

namespace Cardinal.Lessions
{
    /// <summary>
    /// Обучающее задание
    /// </summary>
    public class Lession : ILession
    {
        // Возможно лучше использовать ThreadLocal вместо Lazy

        /// <summary>
        /// Обучающая выборка
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected Lazy<IDatasets> train;

        /// <summary>
        /// Тестовая выборка
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected Lazy<IDatasets> test;

        /// <summary>
        /// Проверочная выборка
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected Lazy<IDatasets> validation;

        /// <summary>
        /// Реперзентативные данные
        /// </summary>
        public IDataset[] Dataset
        {
            set
            {
                // TODO реализовать алгоритм разделения данных на части
                var lazy = new Lazy<IDatasets>(() => new Datasets { Values = value });

                train = lazy;
                test = lazy;
                validation = lazy;
            }
        }

        /// <summary>
        /// Обучающая выборка
        /// </summary>
        public IDatasets Train => train.Value;

        /// <summary>
        /// Тестовая выборка
        /// </summary>
        public IDatasets Test => test.Value;

        /// <summary>
        /// Поверочная выборка
        /// </summary>
        public IDatasets Validation => validation.Value;
    }
}