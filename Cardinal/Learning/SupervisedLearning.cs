using System;
using System.Collections.Generic;
using Cardinal.ActivationFunctions.Contract;
using Cardinal.Layers.Contract;
using Cardinal.Learning.Contract;
using Cardinal.Lessions.Contract;
using Cardinal.Networks.Contract;
using Cardinal.Neurons.Contract;

namespace Cardinal.Learning
{
    // Chember

    /// <summary>
    /// Обучение с учителем
    /// </summary>
    public class SupervisedLearning : ISupervisedLearning
    {
        /// <summary>
        /// Нейронная сеть
        /// </summary>
        private readonly INetwork network;

        private IActivationFunction activationFunction;

        /// <summary>
        /// Размер пакета.
        /// </summary>
        private int batchSize = 1;

        /// <summary>
        /// Индекс слоя до выходного
        /// </summary>
        private int beforeOutputLayerIndex;

        //private IData currentData;

        private double[] biasDeltas,
            weightDeltas,
            biasDeltasAcc,
            biasDeltasAccPrev,
            weightDeltasAcc,
            weights,
            input,
            neuronGradients,
            nextLayerGradients;

        /// <summary>
        /// Изменения смещений нейронов(слой, нейрон)
        /// </summary>
        private double[][] biasLayerDeltas;

        /// <summary>
        /// Суммарное изменения смещений нейронов(слой, нейрон)
        /// </summary>
        private double[][] biasLayerDeltasAcc;

        private double[][] biasLayerDeltasAccPrev;
        private double[][] biasLayerUpdates;

        private double[] biasNeuronUpdates;
        private double cachedL1;
        private double cachedL2Sqr;

        private double cachedRegularizationRate;
        private double deltaMax = 50.0;
        private double deltaMin = 1e-6;
        private double error = double.MaxValue, prevError, sum, cachedGradient;

        private double etaMinus = 0.5;
        private double etaPlus = 1.2;

        /// <summary>
        /// Номер веса
        /// </summary>
        private int i;

        /// <summary>
        /// Количество весов
        /// </summary>
        private int ic;

        /// <summary>
        /// Номер нейрона в слое
        /// </summary>
        private int j;

        /// <summary>
        /// Количество нейронов в слое
        /// </summary>
        private int jc;

        /// <summary>
        /// Номер слоя
        /// </summary>
        private int l;

        private double l1;
        private double l2Sqr;

        private ILayer<INeuron> layer;

        /// <summary>
        /// Градиенты(слой, нейрон)
        /// </summary>
        private double[][] layerGradients;


        private double[][] layerGradientsPrev;
        private ILayer<INeuron>[] layers;

        /// <summary>
        /// Количество слоев
        /// </summary>
        private int layersCount;

        /// <summary>
        /// Скорость обучения
        /// </summary>
        private double learningRate = 0.1;

        private double[][] learningRateBiasLayers;

        private double[] learningRateBiasNeurons;

        private double learningRateBonus = 0.05;
        private double learningRatePenalty = 0.95;


        private double[] learningRateWeights;
        private double[][][] learningRateWeightsLayers;
        private double[][] learningRateWeightsNeuronus;

        ///// <summary>
        ///// Импульс обучения (learningRate * momentum)
        ///// </summary>
        //private double cachedMomentum;

        /// <summary>
        /// Импульс.
        /// Используется в онлайн обучении
        /// </summary>
        private double momentum = 0.9; //0.5;

        /// <summary>
        /// Обратный импульс обучения(learningRate * (1 - momentum))
        /// </summary>
        private double momentumPenalty;

        private INeuron neuron;
        private double[] neuronGradientsPrev;
        private INeuron[] neurons, nextLayerNeurons;

        /// <summary>
        /// Индекс выходного слоя
        /// </summary>
        private int outputLayerIndex;

        //private double input;

        ////https://ru.wikipedia.org/wiki/Регуляризация
        /// <summary>
        /// Регуляризация(штраф за переобучение, регрессия).
        /// Используется в оффлайн обучении
        /// </summary>
        private double regularization = 0.01;

        private double S;

        private double squaredDelta;

        private double sumOutputLayerError;

        private double[] weightDeltasAccPrev;

        /// <summary>
        /// Изменения весовых коэфицентов(слой, нейрон, весовой коэфицент)
        /// </summary>
        private double[][][] weightLayerDeltas;

        /// <summary>
        /// Суммарное изменения весовых коэфицентов(слой, нейрон, весовой коэфицент)
        /// </summary>
        private double[][][] weightLayerDeltasAcc;

        private double[][][] weightLayerDeltasAccPrev;
        private double[][][] weightLayerUpdates;

        private double[][] weightNeuronDeltas;
        private double[][] weightNeuronDeltasAcc;
        private double[][] weightNeuronDeltasAccPrev;
        private double[][] weightNeuronUpdates;

        private double[] weightUpdates;

        public SupervisedLearning(INetwork network)
        {
            this.network = network;

            layers = network.Layers;

            layersCount = layers.Length;
            outputLayerIndex = layersCount - 1;
            beforeOutputLayerIndex = outputLayerIndex - 1;

            layerGradients = new double[layersCount][];
            layerGradientsPrev = new double[layersCount][];
            weightLayerDeltas = new double[layersCount][][];
            biasLayerDeltas = new double[layersCount][];

            weightLayerDeltasAcc = new double[layersCount][][];
            weightLayerDeltasAccPrev = new double[layersCount][][];

            biasLayerDeltasAcc = new double[layersCount][];
            biasLayerDeltasAccPrev = new double[layersCount][];

            learningRateWeightsLayers = new double[layersCount][][];
            learningRateBiasLayers = new double[layersCount][];

            weightLayerUpdates = new double[layersCount][][];
            biasLayerUpdates = new double[layersCount][];

            for (l = 0; l < layersCount; l++)
            {
                layerGradients[l] = new double[layers[l].Neurons.Length];
                layerGradientsPrev[l] = new double[layers[l].Neurons.Length];

                weightLayerDeltas[l] = new double[layers[l].Neurons.Length][];
                biasLayerDeltas[l] = new double[layers[l].Neurons.Length];

                weightLayerDeltasAcc[l] = new double[layers[l].Neurons.Length][];
                weightLayerDeltasAccPrev[l] = new double[layers[l].Neurons.Length][];

                biasLayerDeltasAcc[l] = new double[layers[l].Neurons.Length];
                biasLayerDeltasAccPrev[l] = new double[layers[l].Neurons.Length];

                learningRateWeightsLayers[l] = new double[layers[l].Neurons.Length][];
                learningRateBiasLayers[l] = new double[layers[l].Neurons.Length];

                weightLayerUpdates[l] = new double[layers[l].Neurons.Length][];
                biasLayerUpdates[l] = new double[layers[l].Neurons.Length];


                weightNeuronDeltas = weightLayerDeltas[l];
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                weightNeuronDeltasAccPrev = weightLayerDeltasAccPrev[l];
                learningRateWeightsNeuronus = learningRateWeightsLayers[l];
                weightNeuronUpdates = weightLayerUpdates[l];

                for (j = 0, jc = weightNeuronDeltas.Length; j < jc; j++)
                {
                    weightNeuronDeltas[j] = new double[layers[l].Inputs];
                    weightNeuronDeltasAcc[j] = new double[layers[l].Inputs];

                    learningRateWeightsNeuronus[j] = new double[layers[l].Inputs];
                    weightNeuronDeltasAccPrev[j] = new double[layers[l].Inputs];

                    weightNeuronUpdates[j] = new double[layers[l].Inputs];
                }
            }

            ResetLearningRate();
        }

        /// <summary>
        /// Размер пакета.
        /// </summary>
        private int BatchSize
        {
            get { return batchSize; }
            set
            {
                batchSize = value;
                UpdateCachedRegularizationRate();
            }
        }

        /// <summary>
        /// Регуляризация(штраф за переобучение, регрессия).
        /// Используется в оффлайн обучении
        /// </summary>
        public double Regularization
        {
            get { return regularization; }
            set
            {
                regularization = value;
                UpdateCachedRegularizationRate();
            }
        }

        /// <summary>
        /// Нейронная сеть
        /// </summary>
        public INetwork Network => network;

        /// <summary>
        /// Скорость обучения
        /// </summary>
        public double LearningRate
        {
            get { return learningRate; }
            set
            {
                if (value < 0) throw new ArgumentException();
                if (value > 1) throw new ArgumentException();
                learningRate = value;
                //cachedMomentum = learningRate * momentum;
                //cachedMomentumPenalty = learningRate * (1 - momentum);
                momentumPenalty = 1 - momentum;
                //UpdateCachedRegularizationRate();
            }
        }

        /// <summary>
        /// Импульс.
        /// Используется в онлайн обучении
        /// </summary>
        public double Momentum
        {
            get { return momentum; }
            set
            {
                if (value < 0) throw new ArgumentException();
                if (value > 1) throw new ArgumentException();
                momentum = value;
                //cachedMomentum = learningRate * momentum;
                //cachedMomentumPenalty = learningRate * (1 - momentum);
            }
        }

        //private double r, q;

        public double LearningRateBonus
        {
            get { return learningRateBonus; }
            set
            {
                if (value > 1) throw new ArgumentException();
                if (value < 0) throw new ArgumentException();

                learningRateBonus = value;
                learningRatePenalty = 1 - learningRateBonus;
                //q = value;
                //r = Math.Exp(q);
            }
        }

        /// <summary>
        /// Онлайн обучение
        /// </summary>
        /// <param name="datasets">Обучающая выборка</param>
        /// <returns>Ошибка сети</returns>
        public void OnlineLearn(IEnumerable<IDataset> datasets)
        {
            //ResetUpdates(1);

            foreach (var dataset in datasets)
            {
                network.Compute(dataset.Input);

                prevError = error;
                error = ComputeError(dataset.Desired);

                ComputeGradients(dataset.Desired);

                ResetAcc();

                ComputeOnlineAdaptiveLearnRate();

                ComputeOnlineDeltas(dataset.Input);

                ComputeDeltas();
                UpdateWeights();

                //ComputeDeltasWithoutPrev();

                //UpdateWeightsOptimized();
            }
        }

        /// <summary>
        /// Оффлайн обучение(пакетное)
        /// </summary>
        /// <param name="datasets">Обучающая выборка</param>
        /// <param name="batchSize">Размер обучающей выборки</param>
        public void OfflineLearn(IEnumerable<IDataset> datasets, int batchSize)
        {
            ResetAcc();
            ResetDeltas();
            //ResetUpdates(1);

            BatchSize = batchSize;
            prevError = error;
            error = 0;

            foreach (var dataset in datasets)
            {
                network.Compute(dataset.Input);

                error += ComputeError(dataset.Desired);

                ComputeGradients(dataset.Desired);

                ComputeOfflineAdaptiveLearnRate();

                //ComputeOfflineRegularization();

                ComputeOfflineDeltas(dataset.Input);
            }

            ComputeDeltas();
            UpdateWeights();

            //ComputeDeltasWithoutPrev();
            //UpdateWeightsOptimized();
        }

        private void UpdateCachedRegularizationRate()
        {
            cachedRegularizationRate = /*learningRate * */ regularization / batchSize;
        }

        private void ResetDeltas()
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronDeltas = weightLayerDeltas[l];
                for (j = 0, jc = weightNeuronDeltas.Length; j < jc; j++)
                {
                    Array.Clear(weightNeuronDeltas[j], 0, weightNeuronDeltas[j].Length);
                }
                Array.Clear(biasLayerDeltas[l], 0, biasLayerDeltas[l].Length);
            }
        }

        private void ResetAcc()
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                for (j = 0, jc = weightNeuronDeltasAcc.Length; j < jc; j++)
                {
                    Array.Clear(weightNeuronDeltasAcc[j], 0, weightNeuronDeltasAcc[j].Length);
                }
                Array.Clear(biasLayerDeltasAcc[l], 0, biasLayerDeltasAcc[l].Length);
            }
        }

        private void ResetLearningRate()
        {
            for (l = 0; l < layersCount; l++)
            {
                learningRateWeightsNeuronus = learningRateWeightsLayers[l];
                learningRateBiasNeurons = learningRateBiasLayers[l];

                for (j = 0, jc = learningRateWeightsNeuronus.Length; j < jc; j++)
                {
                    learningRateWeights = learningRateWeightsNeuronus[j];

                    for (i = 0, ic = learningRateWeights.Length; i < ic; i++)
                    {
                        learningRateWeights[i] = 1.0;
                    }

                    learningRateBiasNeurons[j] = 1.0;
                }
            }
        }

        private void ResetUpdates(double rate)
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronUpdates = weightLayerUpdates[l];
                biasNeuronUpdates = biasLayerUpdates[l];

                for (j = 0, jc = weightNeuronUpdates.Length; j < jc; j++)
                {
                    weightUpdates = weightNeuronUpdates[j];

                    for (i = 0, ic = weightUpdates.Length; i < ic; i++)
                    {
                        weightUpdates[i] = rate;
                    }

                    biasNeuronUpdates[j] = rate;
                }
            }
        }

        /// <summary>
        /// Вычислить ошибку
        /// </summary>
        /// <param name="desired">Желаемый результат(t)</param>
        /// <returns>Ошибка сети</returns>
        private double ComputeError(double[] desired)
        {
            layer = layers[outputLayerIndex];
            input = layer.Output;

            //TODO вынести в аттестацию
            // вычисляем ошибку в нейронах выходного слоя
            // приходится вычислять каждый раз азного до обновления весов
            // хотя она нужна только перед запуском обучения
            // TODO добавить возможность определять градиент по ошибке(заменять функцию активации функцией ошибки)
            return network.ErrorFunction.Compute(desired, input) / 2; // / desired.Length;// * 0.5;//
        }

        // https://www.quora.com/Whats-the-difference-between-gradient-descent-and-stochastic-gradient-descent
        /// <summary>
        /// Вычислить градиен
        /// </summary>
        /// <param name="desired">Желаемый результат(t)</param>
        private void ComputeGradients(double[] desired)
        {
            layer = layers[outputLayerIndex];
            input = layer.Output;
            activationFunction = layer.ActivationFunction;
            neuronGradients = layerGradients[outputLayerIndex];
            //neuronGradientsPrev = layerGradientsPrev[outputLayerIndex];//!!!

            //вычисляем градиент выходного слоя
            for (j = 0, jc = layer.Neurons.Length; j < jc; j++)
            {
                //neuronGradientsPrev[j] = neuronGradients[j]; //!!!
                neuronGradients[j] = activationFunction.Derivative2(input[j]) * (desired[j] - input[j]);
                //neuronGradients[j] = desired[j] - input[j];
            }

            //вычисляем градиент в нейронах входного и скрытых слоёв
            for (l = beforeOutputLayerIndex; l >= 0; l--)
            {
                layer = layers[l];
                activationFunction = layer.ActivationFunction;
                input = layer.Output;
                neuronGradients = layerGradients[l];
                //neuronGradientsPrev = layerGradientsPrev[l];//!!!
                nextLayerNeurons = layers[l + 1].Neurons;
                nextLayerGradients = layerGradients[l + 1];

                for (j = 0, jc = layer.Neurons.Length; j < jc; j++)
                {
                    //TODO переделать
                    sum = 0.0;
                    for (i = 0, ic = nextLayerNeurons.Length; i < ic; i++)
                    {
                        // dot произведение
                        sum += nextLayerGradients[i] * nextLayerNeurons[i].Weights[j];
                    }
                    //neuronGradientsPrev[j] = neuronGradients[j];//!!!
                    neuronGradients[j] = activationFunction.Derivative2(input[j]) * sum;
                }
            }
        }

        // https://habrahabr.ru/post/157179/
        // https://www.willamette.edu/~gorr/classes/cs449/momrate.html
        /// <summary>
        /// Расчёт адаптивной скорости обучения для каждого весового коэфицента в оффлайн поиске(пачками)
        /// </summary>
        private void ComputeOfflineAdaptiveLearnRate()
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                weightNeuronDeltasAccPrev = weightLayerDeltasAccPrev[l];

                biasDeltasAcc = biasLayerDeltasAcc[l];
                biasDeltasAccPrev = biasLayerDeltasAccPrev[l];

                learningRateWeightsNeuronus = learningRateWeightsLayers[l];
                learningRateBiasNeurons = learningRateBiasLayers[l];

                for (j = 0, jc = learningRateWeightsNeuronus.Length; j < jc; j++)
                {
                    learningRateWeights = learningRateWeightsNeuronus[j];
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    weightDeltasAccPrev = weightNeuronDeltasAccPrev[j];

                    for (i = 0, ic = learningRateWeights.Length; i < ic; i++)
                    {
                        // TODO вынести в функцию оптимизации
                        if (weightDeltasAcc[i] * weightDeltasAccPrev[i] > 0)
                        {
                            learningRateWeights[i] = Math.Max(0.001, learningRateWeights[i] * learningRatePenalty);
                        }
                        else
                        {
                            learningRateWeights[i] = Math.Min(100, learningRateWeights[i] + learningRateBonus);
                        }
                    }

                    if (biasDeltasAcc[j] * biasDeltasAccPrev[j] > 0)
                    {
                        learningRateBiasNeurons[j] = Math.Max(0.01, learningRateBiasNeurons[j] * learningRatePenalty);
                    }
                    else
                    {
                        learningRateBiasNeurons[j] = Math.Min(100, learningRateBiasNeurons[j] + learningRateBonus);
                    }
                }
            }
        }

        // https://www.willamette.edu/~gorr/classes/cs449/momrate.html
        /// <summary>
        /// Расчёт адаптивной скорости обучения для каждого весового коэфицента в онлайн поиске(подряд по одному примеру)
        /// </summary>
        private void ComputeOnlineAdaptiveLearnRate()
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                weightNeuronDeltasAccPrev = weightLayerDeltasAccPrev[l];

                biasDeltasAcc = biasLayerDeltasAcc[l];
                biasDeltasAccPrev = biasLayerDeltasAccPrev[l];

                learningRateWeightsNeuronus = learningRateWeightsLayers[l];
                learningRateBiasNeurons = learningRateBiasLayers[l];

                for (j = 0, jc = learningRateWeightsNeuronus.Length; j < jc; j++)
                {
                    learningRateWeights = learningRateWeightsNeuronus[j];
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    weightDeltasAccPrev = weightNeuronDeltasAccPrev[j];

                    for (i = 0, ic = learningRateWeights.Length; i < ic; i++)
                    {
                        squaredDelta = weightDeltasAcc[i] * weightDeltasAcc[i];

                        // ходовая проверка на 0
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        if (squaredDelta != 0)
                        {
                            // линейная нормализация
                            learningRateWeights[i] = learningRateWeights[i] *
                                                     Math.Max(0.5,
                                                         1 + learningRateBonus +
                                                         weightDeltasAcc[i] * (weightDeltasAccPrev[i] / squaredDelta));
                        }
                        else
                        {
                            learningRateWeights[i] = 1.0;
                        }
                    }

                    squaredDelta = biasDeltasAcc[j] * biasDeltasAcc[j];

                    // ходовая проверка на 0
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (squaredDelta != 0)
                    {
                        // линейная нормализация
                        learningRateBiasNeurons[j] = learningRateBiasNeurons[j] *
                                                     Math.Max(0.5,
                                                         1 + learningRateBonus +
                                                         biasDeltasAcc[j] * (biasDeltasAccPrev[j] / squaredDelta));
                    }
                    else
                    {
                        learningRateBiasNeurons[j] = 1.0;
                    }
                }
            }
        }

        // https://habrahabr.ru/post/154369/
        /// <summary>
        /// Расчёт регуляризации(насколько мне известно применим только к оффлайн поиску)
        /// </summary>
        private void ComputeOfflineRegularization()
        {
            l1 = 0;
            l2Sqr = 0;

            for (l = 0; l < layersCount; l++)
            {
                neurons = layers[l].Neurons;
                for (j = 0, jc = neurons.Length; j < jc; j++)
                {
                    weights = neurons[j].Weights;

                    for (i = 0, ic = weights.Length; i < ic; i++)
                    {
                        l1 += Math.Abs(weights[i]);
                        l2Sqr += weights[i] * weights[i];
                    }
                }
            }
            cachedL1 = cachedRegularizationRate * l1;
            cachedL2Sqr = cachedRegularizationRate * l2Sqr;
        }


        /// <summary>
        /// Онлайн расчёт сдвигов весовых коэфицентов
        /// </summary>
        /// <param name="input">Входной вектор</param>
        private void ComputeOnlineDeltas(double[] input)
        {
            // на первом шаге считаем изменения весовых коэфицентов и смешений нейронов на входном слое(по специальной формуле для входного слоя)
            // где вывод подаваемый на вход слоя - значения подаваемые на вход сети(т.к. предидущего слоя нет)
            this.input = input;

            for (l = 0; l < layersCount; l++)
            {
                neuronGradients = layerGradients[l];
                weightNeuronDeltas = weightLayerDeltas[l];
                biasDeltas = biasLayerDeltas[l];
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                biasDeltasAcc = biasLayerDeltasAcc[l];
                neurons = layers[l].Neurons;

                for (j = 0, jc = weightNeuronDeltas.Length; j < jc; j++)
                {
                    weightDeltas = weightNeuronDeltas[j];
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    neuron = neurons[j];
                    weights = neuron.Weights;

                    cachedGradient = momentumPenalty * neuronGradients[j];

                    for (i = 0, ic = weightDeltas.Length; i < ic; i++)
                    {
                        weightDeltas[i] = momentum * weightDeltas[i] + cachedGradient * this.input[i];
                        weightDeltasAcc[i] += weightDeltas[i];
                    }

                    biasDeltas[j] = momentum * biasDeltas[j] + cachedGradient;
                    biasDeltasAcc[j] += biasDeltas[j];
                }

                // на последующих шагах считаем изменения весовых коэфицентов и смешений нейронов на всех прочих слоях (не входном, по специальной формуле для прочих слёв)
                // где вывод подаваемый на вход слоя - значения выхода предидущего слоя
                this.input = layers[l].Output;
            }
        }

        /// <summary>
        /// Оффлайн расчёт сдвигов весовых коэфицентов
        /// </summary>
        /// <param name="input">Входной вектор</param>
        private void ComputeOfflineDeltas(double[] input)
        {
            // на первом шаге считаем изменения весовых коэфицентов и смешений нейронов на входном слое(по специальной формуле для входного слоя)
            // где вывод подаваемый на вход слоя - значения подаваемые на вход сети(т.к. предидущего слоя нет)
            this.input = input;

            for (l = 0; l < layersCount; l++)
            {
                neuronGradients = layerGradients[l];
                neuronGradientsPrev = layerGradientsPrev[l];
                weightNeuronDeltas = weightLayerDeltas[l];
                biasDeltas = biasLayerDeltas[l];
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                biasDeltasAcc = biasLayerDeltasAcc[l];
                neurons = layers[l].Neurons;
                learningRateWeightsNeuronus = learningRateWeightsLayers[l];

                for (j = 0, jc = weightNeuronDeltas.Length; j < jc; j++)
                {
                    weightDeltas = weightNeuronDeltas[j];
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    weights = neurons[j].Weights;
                    learningRateWeights = learningRateWeightsNeuronus[j];

                    cachedGradient = neuronGradients[j];

                    for (i = 0, ic = weightDeltas.Length; i < ic; i++)
                    {
                        // не уверен что регуляризация используется именно тут(помойму второй уровень используют в расчёте ошибке)
                        // не совсем уверен что можно использовать сразу два уровня регуляризации

                        // с двумя нормами регуляризации ищет значительно быстрее, но шанс успеха ниже
                        weightDeltas[i] = cachedGradient * this.input[i] + cachedL2Sqr / weights[i] + cachedL1 / weights[i];
                        weightDeltasAcc[i] += weightDeltas[i];
                    }

                    biasDeltas[j] = cachedGradient;
                    biasDeltasAcc[j] += biasDeltas[j];
                }

                // на последующих шагах считаем изменения весовых коэфицентов и смешений нейронов на всех прочих слоях (не входном, по специальной формуле для прочих слёв)
                // где вывод подаваемый на вход слоя - значения выхода предидущего слоя
                this.input = layers[l].Output;
            }
        }

        //private int interation = 0;
        //private double hanningLearnRate = 0;

        /// <summary>
        /// Финальный расчёт сдвигов весовых коэфицентов.
        /// Умножение на скорость обучения
        /// </summary>
        private void ComputeDeltas()
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];

                weightNeuronDeltasAccPrev = weightLayerDeltasAccPrev[l]; //!!
                biasDeltasAcc = biasLayerDeltasAcc[l];
                biasDeltasAccPrev = biasLayerDeltasAccPrev[l]; //!!
                learningRateWeightsNeuronus = learningRateWeightsLayers[l];
                learningRateBiasNeurons = learningRateBiasLayers[l];

                for (j = 0, jc = weightNeuronDeltasAcc.Length; j < jc; j++)
                {
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    weightDeltasAccPrev = weightNeuronDeltasAccPrev[j]; //!!
                    learningRateWeights = learningRateWeightsNeuronus[j];
                    //hanningLearnRate = (0.5d - Math.Abs(0.5d * Math.Cos((360d*interation)/3d)))/1.5d;
                    //interation++;

                    for (i = 0, ic = weightDeltasAcc.Length; i < ic; i++)
                    {
                        weightDeltasAccPrev[i] = weightDeltasAcc[i]; //!!

                        weightDeltasAcc[i] = learningRate * learningRateWeights[i] * weightDeltasAcc[i];
                        //* hanningLearnRate 

                        // если где то в алгоритмах будут использоваться weightDeltas то возможно тут тоже необходимо применить к нему скорость обучения
                    }

                    biasDeltasAccPrev[j] = biasDeltasAcc[j]; //!!
                    biasDeltasAcc[j] = learningRate * learningRateBiasNeurons[j] * biasDeltasAcc[j];
                }
            }
        }

        private void ComputeDeltasWithoutPrev()
        {
            for (l = 0; l < layersCount; l++)
            {
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];

                //weightNeuronDeltasAccPrev = weightLayerDeltasAccPrev[l];//!!
                biasDeltasAcc = biasLayerDeltasAcc[l];
                //biasDeltasAccPrev = biasLayerDeltasAccPrev[l];//!!
                learningRateWeightsNeuronus = learningRateWeightsLayers[l];
                learningRateBiasNeurons = learningRateBiasLayers[l];

                for (j = 0, jc = weightNeuronDeltasAcc.Length; j < jc; j++)
                {
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    //weightDeltasAccPrev = weightNeuronDeltasAccPrev[j];//!!
                    learningRateWeights = learningRateWeightsNeuronus[j];
                    //hanningLearnRate = (0.5d - Math.Abs(0.5d * Math.Cos((360d*interation)/3d)))/1.5d;
                    //interation++;

                    for (i = 0, ic = weightDeltasAcc.Length; i < ic; i++)
                    {
                        //weightDeltasAccPrev[i] = weightDeltasAcc[i];//!!

                        weightDeltasAcc[i] = learningRate * learningRateWeights[i] * weightDeltasAcc[i];
                        //* hanningLearnRate 

                        // если где то в алгоритмах будут использоваться weightDeltas то возможно тут тоже необходимо применить к нему скорость обучения
                    }

                    //biasDeltasAccPrev[j] = biasDeltasAcc[j];//!!
                    biasDeltasAcc[j] = learningRate * learningRateBiasNeurons[j] * biasDeltasAcc[j];
                }
            }
        }

        /// <summary>
        /// Обновить весовые коэфиценты и смещения сети
        /// </summary>
        private void UpdateWeights()
        {
            for (l = 0; l < layersCount; l++)
            {
                neurons = layers[l].Neurons;
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                biasDeltasAcc = biasLayerDeltasAcc[l];

                for (j = 0, jc = neurons.Length; j < jc; j++)
                {
                    weights = neurons[j].Weights;
                    weightDeltasAcc = weightNeuronDeltasAcc[j];

                    for (i = 0, ic = weights.Length; i < ic; i++)
                    {
                        weights[i] += weightDeltasAcc[i];
                    }

                    neurons[j].Bias += biasDeltasAcc[j];
                }
            }
        }

        private void UpdateWeightsOptimized()
        {
            for (l = 0; l < layersCount; l++)
            {
                neurons = layers[l].Neurons;
                weightNeuronDeltasAcc = weightLayerDeltasAcc[l];
                weightNeuronDeltasAccPrev = weightLayerDeltasAccPrev[l];
                biasDeltasAcc = biasLayerDeltasAcc[l];
                biasDeltasAccPrev = biasLayerDeltasAccPrev[l];

                weightNeuronUpdates = weightLayerUpdates[l];
                biasNeuronUpdates = biasLayerUpdates[l];

                neuronGradients = layerGradients[l];
                neuronGradientsPrev = layerGradientsPrev[l];

                for (j = 0, jc = neurons.Length; j < jc; j++)
                {
                    neuron = neurons[j];
                    weights = neuron.Weights;
                    weightDeltasAcc = weightNeuronDeltasAcc[j];
                    weightDeltasAccPrev = weightNeuronDeltasAccPrev[j];
                    weightUpdates = weightNeuronUpdates[j];

                    //S = neuronGradientsPrev[j]*neuronGradients[j];

                    for (i = 0, ic = weights.Length; i < ic; i++)
                    {
                        S = weightDeltasAccPrev[i] * weightDeltasAcc[i];

                        if (S > 0)
                        {
                            weightUpdates[i] = Math.Min(weightUpdates[i] * etaPlus, deltaMax);
                            weights[i] -= Math.Sign(weightDeltasAcc[i]) * weightUpdates[i];
                            weightDeltasAccPrev[i] = weightDeltasAcc[i];
                        }
                        else if (S < 0)
                        {
                            weightUpdates[i] = Math.Max(weightUpdates[i] * etaMinus, deltaMin);

                            //if (prevError > error)
                            //{
                            //    weights[i] += Math.Sign(weightDeltasAcc[i]) * weightUpdates[i];
                            //}

                            weightDeltasAccPrev[i] = 0;
                        }
                        else
                        {
                            weights[i] -= Math.Sign(weightDeltasAcc[i]) * weightUpdates[i];
                            weightDeltasAccPrev[i] = weightDeltasAcc[i];
                        }
                    }
                    neuron.Bias += biasDeltasAcc[j];
                    //S = biasDeltasAccPrev[j] * biasDeltasAcc[j];

                    //if (S > 0)
                    //{
                    //    biasNeuronUpdates[j] = Math.Min(biasNeuronUpdates[j] * etaPlus, deltaMax);
                    //    neuron.Bias -= Math.Sign(biasDeltasAcc[j]) * biasNeuronUpdates[j];
                    //    biasDeltasAccPrev[j] = biasDeltasAcc[j];
                    //}
                    //else if (S < 0)
                    //{
                    //    biasNeuronUpdates[j] = Math.Max(biasNeuronUpdates[j] * etaMinus, deltaMin);

                    //    //if (prevError > error)
                    //    //{
                    //    //    neuron.Bias += Math.Sign(biasDeltasAcc[j]) * biasNeuronUpdates[j];
                    //    //}

                    //    biasDeltasAccPrev[j] = 0;
                    //}
                    //else
                    //{
                    //    neuron.Bias -= Math.Sign(biasDeltasAcc[j]) * biasNeuronUpdates[j];
                    //    biasDeltasAccPrev[j] = biasDeltasAcc[j];
                    //}
                }
            }
        }
    }
}