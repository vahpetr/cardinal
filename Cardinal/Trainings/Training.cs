using System;
using System.Collections.Generic;
using Cardinal.ErrorFunctions.Contratc;
using Cardinal.Learning.Contract;
using Cardinal.Lessions.Contract;
using Cardinal.Networks.Contract;
using Cardinal.Trainings.Contract;

namespace Cardinal.Trainings
{
    /// <summary>
    /// Тренировочный алгоритм
    /// </summary>
    public class Training : ITraining
    {
        private int batchSize = 1;

        private int maxEpoch = 1000;

        private double minDeltaError = 1e-10;//5e-3;

        private double minError = 0.025;//1e-2;//0.025;//2e-2; //0.01 - 0.1// 0.02;

        private int epoch;

        private readonly ISupervisedLearning learning;

        private readonly ILession lesson;
        private readonly INetwork network;

        private readonly IErrorFunction errorFunction;

        /// <summary>
        /// Конструктор тренировочного алгоритма
        /// </summary>
        /// <param name="learning"></param>
        /// <param name="lesson"></param>
        public Training(ISupervisedLearning learning, ILession lesson)
        {
            this.learning = learning;
            network = learning.Network;

            this.lesson = lesson;
            errorFunction = learning.Network.ErrorFunction;
        }

        /// <summary>
        /// Алгоритм обучения
        /// </summary>
        public ISupervisedLearning Learning => learning;

        /// <summary>
        /// Текущая эпоха
        /// </summary>
        public ILession Lesson => lesson;

        /// <summary>
        /// Текущая эпоха
        /// </summary>
        public int Epoch => epoch;

        /// <summary>
        /// Максимум эпох
        /// </summary>
        public int MaxEpoches
        {
            get { return maxEpoch; }
            set { maxEpoch = value; }
        }

        public CopmutedError TrainError { get; set; } = new CopmutedError();
        public CopmutedError TestError { get; set; } = new CopmutedError();
        public CopmutedError ValidationError { get; set; } = new CopmutedError();

        /// <summary>
        /// Минимальная ошибка
        /// </summary>
        public double MinError
        {
            get { return minError; }
            set { minError = value; }
        }

        /// <summary>
        /// Минимальная дельта 
        /// </summary>
        public double MinDeltaError
        {
            get { return minDeltaError; }
            set { minDeltaError = value; }
        }

        /// <summary>
        /// Размер набора обучения
        /// </summary>
        public int BatchSize
        {
            get { return batchSize; }
            set
            {
                if (value < 1) throw new ArgumentException();
                batchSize = value;
            }
        }

        /// <summary>
        /// Запустить тренинг
        /// </summary>
        /// <returns>Ошибка на валидационной выборке</returns>
        public void Run()
        {
            while (epoch < maxEpoch)
            {
                TestError.PrevValue = TestError.Value;
                TestError.Value = ComputeError(lesson.Test);

                if (TestError.IsInfinite()) break;
                if (TestError.Value < minError) break;
                if (TestError.Delta < minDeltaError) break;
                //if (TestError.PrevValue < TestError.Value / 2) break;
                if (!lesson.Train.HasValues()) break;

                lesson.Train.Shuffle();

                // TODO избавиться от выражения(всегда использовать Get)
                if (batchSize == 1)
                {
                    learning.OnlineLearn(lesson.Train);
                }
                else
                {
                    learning.OfflineLearn(lesson.Train.Get(batchSize), batchSize);
                }

                epoch++;
            }

            ValidationError.PrevValue = TestError.PrevValue;
            ValidationError.Value = ComputeError(lesson.Validation);
        }

        private double tempError;
        private double ComputeError(IEnumerable<IDataset> datasets)
        {
            tempError = 0;
            foreach (var dataset in datasets)
            {
                tempError += errorFunction.Compute(dataset.Desired, network.Compute(dataset.Input)) / 2;
            }
            return tempError;
        }
    }
}