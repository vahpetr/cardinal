using Cardinal.ActivationFunctions.Contract;
using Cardinal.Neurons.Contract;

namespace Cardinal.Layers.Contract
{
    /// <summary>
    /// Интерфейс слоя нейронной сети
    /// </summary>
    /// <typeparam name="TNeuron"></typeparam>
    public interface ILayer<out TNeuron>
        where TNeuron : INeuron
    {
        /// <summary>
        /// Количество входов слоя
        /// </summary>
        int Inputs { get; }

        /// <summary>
        /// Количество выходов слоя
        /// </summary>
        int Outputs { get; }

        /// <summary>
        /// Функция активации
        /// </summary>
        IActivationFunction ActivationFunction { get; set; }

        ///// <summary>
        ///// Выходной вектор суммы слоя
        ///// TODO возможно можно будет удалить
        ///// </summary>
        //double[] Sum { get; }

        /// <summary>
        /// Выходной вектор сигнала слоя
        /// </summary>
        double[] Output { get; }

        /// <summary>
        /// Нейроны
        /// </summary>
        TNeuron[] Neurons { get; }

        /// <summary>
        /// Вычислить выходной сигнал
        /// </summary>
        /// <param name="input">Входной сигнал</param>
        /// <returns>Выходной сигнал</returns>
        double[] Compute(double[] input);
    }
}