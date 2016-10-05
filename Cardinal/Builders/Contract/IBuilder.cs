using Cardinal.ActivationFunctions.Contract;
using Cardinal.ErrorFunctions.Contratc;
using Cardinal.Lessions.Contract;
using Cardinal.Networks.Contract;

namespace Cardinal.Builders.Contract
{
    /// <summary>
    /// Создаёт структуру сети
    /// </summary>
    public interface IBuilder
    {
        IErrorFunction ErrorFunction { get; set; }
        IActivationFunction ActivationFunction { get; set; }
        IDatasets Datasets { get; set; }
        int[] HiddenTopology { get; set; }
        /// <summary>
        /// Построить сеть
        /// </summary>
        /// <returns>Слои сети</returns>
        INetwork Build();
    }
}