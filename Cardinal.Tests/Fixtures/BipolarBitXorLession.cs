using Cardinal.Lessions;
using Cardinal.Lessions.Contract;

namespace Cardinal.Tests.Fixtures
{
    /// <summary>
    /// Логическое исключающее или. ИЛИ НЕ. ^. XOR. Биполярное
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BipolarBitXorLession : Lession
    {
        public BipolarBitXorLession()
        {
            Dataset = new IDataset[]
            {
                new Dataset(new double[] {-1, -1}, new double[] {-1}),
                new Dataset(new double[] {-1, 1}, new double[] {1}),
                new Dataset(new double[] {1, -1}, new double[] {1}),
                new Dataset(new double[] {1, 1}, new double[] {-1})
            };
        }
    }
}