using Cardinal.ErrorFunctions.Contratc;
using Cardinal.Layers.Contract;
using Cardinal.Neurons.Contract;

namespace Cardinal.Networks.Contract
{
    /// <summary>
    /// Интерфейс нейронной сети
    /// </summary>
    public interface INetwork
    {
        /// <summary>
        /// Слои
        /// </summary>
        ILayer<INeuron>[] Layers { get; }

        /// <summary>
        /// Количество входов сети
        /// </summary>
        int Inputs { get; }

        /// <summary>
        /// Количество выходов сети
        /// </summary>
        int Outputs { get; }

        /// <summary>
        /// Выходной вектор сети
        /// </summary>
        double[] Output { get; }

        /// <summary>
        /// Вычислить выходной сигнал
        /// </summary>
        /// <param name="input">Входной сигнал</param>
        /// <returns>Выходной сигнал</returns>
        double[] Compute(double[] input);

        IErrorFunction ErrorFunction { get; set; }
    }
}